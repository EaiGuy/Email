namespace EaiGuy.Email
{
    using log4net;
    using Messages.Commands;
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    public class EmailProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailProvider));
        private static readonly string HostName = System.Environment.MachineName;
        private readonly EmailSettings Settings;

        public EmailProvider(EmailSettings settings)
        {
            this.Settings = settings;
        }

        public void SendEmail(EmailCommand cmd)
        {
            log.InfoFormat("Processing a SendEmail command for '{0}' titled '{1}'", String.Join(", ", cmd.ToAddresses), cmd.Subject);

            // add the host name to the beginning of the subject for non-production environments
            string hostName = Environment.MachineName;
            string hostString = String.Empty;
            if (String.IsNullOrEmpty(this.Settings.ProductionHostRegex) || !Regex.IsMatch(hostName, Settings.ProductionHostRegex))
            {
                hostString = hostName + " - ";
            }

            // add a line across the bottom of emails to separate email bodids from confidentiality notices injected by a company's mail provider
            var msg = new MailMessage()
            {
                From = new MailAddress(this.Settings.FromAddress, this.Settings.FromDisplayName),
                Subject = hostString + cmd.Subject,
                Body = Regex.Replace(cmd.Body, "(\r|\n|\r\n)", "<p>") + "<p><hr><p>",
                IsBodyHtml = true,
            };

            if (cmd.Format == EmailFormat.PlainText)
            {
                msg.IsBodyHtml = false;
                msg.Body = msg.Body.Replace("<br />", "\r\n")
                    .Replace("<p>", "\r\n")
                    .Replace("<hr>", "-------------------------------------------------------------------");
            }

            foreach (string addr in cmd.ToAddresses) msg.To.Add(addr);

            using (var client = new SmtpClient(this.Settings.SmtpServer))
            {
                if (this.Settings.DisableSendingEmails) log.Info("Sending emails is disabled. This email will not be sent.");
                else
                {
                    log.InfoFormat("Sending email to '{0}' titled '{1}'", String.Join(", ", msg.To.First().Address), msg.Subject);
                    try
                    {
                        client.Send(msg);
                    }
                    catch (Exception e)
                    {
                        log.Error("Error sending email: " + e.ToString());
                        throw;
                    }
                }

                // if specified in config, write the email to a file
                if (!String.IsNullOrWhiteSpace(this.Settings.WriteACopyOfAllEmailsToThisFolder))
                {
                    log.Info("Writing the email to a file, since WriteACopyOfAllEmailsToThisFolder config value is populated.");
                    client.PickupDirectoryLocation = this.Settings.WriteACopyOfAllEmailsToThisFolder;
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

                    try
                    {
                        client.Send(msg);
                    }
                    catch (Exception e)
                    {
                        // if there is an exception writing the email to a file, swallow the exception
                        // so we don't go into automatic retries of the entire message handler and 
                        // potentially send duplicate SMTP emails above
                        log.Error("Error writing email to file: " + e.ToString());
                    }
                }
            }
        }
    }
}
