namespace EaiGuy.Email.Messages.Commands
{
    using System;
    using System.Linq;

    public class EmailCommand
    {
        public EmailCommand(string[] toAddresses, string subject, string body, EmailFormat format = EmailFormat.Html)
        {
            this.ToAddresses = toAddresses;
            this.Subject = subject;
            this.Body = body;
            this.Format = format;

            this.Validate();
        }

        private void Validate()
        {
            if (this.ToAddresses == null || !this.ToAddresses.Any())
            {
                throw new ApplicationException("You must specify an email address.");
            }
        }

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="toAddresses">List of emails separated by ',', ';', or ' '</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public EmailCommand(string toAddresses, string subject, string body, EmailFormat format = EmailFormat.Html) :
            this(toAddresses.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries), subject, body)
        { }

        public string[] ToAddresses { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public EmailFormat Format { get; private set; }
    }

    public enum EmailFormat
    {
        Html,
        PlainText,
    }
}