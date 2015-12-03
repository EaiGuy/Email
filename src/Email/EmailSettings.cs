namespace EaiGuy.Email
{
    using System;
    using System.Configuration;

    public class EmailSettings
    {
        public string FromAddress { get { return ConfigurationManager.AppSettings["FromAddress"]; } }
        public string FromDisplayName { get { return ConfigurationManager.AppSettings["FromDisplayName"]; } }
        public string SmtpServer { get { return ConfigurationManager.AppSettings["SmtpServer"]; } }

        /// <summary>
        /// If set, the SendEmail method will write a copy of the .eml file to the specified folder 
        /// (in addition to sending the email, depending on whether DisableSendingEmails is set to true or false).
        /// </summary>
        public string WriteACopyOfAllEmailsToThisFolder { get { return ConfigurationManager.AppSettings["WriteACopyOfAllEmailsToThisFolder"]; } }

        /// <summary>
        /// If false, the SendEmail method will not send the email. This is often used in conjunction with the 
        /// WriteACopyOfAllEmailsToThisFolder setting to prevent sending emails to unintended recipients during 
        /// development and testing phases.
        /// </summary>
        public bool DisableSendingEmails { get { return Boolean.Parse(ConfigurationManager.AppSettings["DisableSendingEmails"]); } }

        /// <summary>
        /// Provide a regular expression that matches the production host (on which the SendEmail command will be called)
        /// to prevent the SendEmail method from prepending the hostname to production email subjects. Emails sent from 
        /// any host not matching this regex will include the hostname in the subject to clarify which environment is 
        /// generating the email.
        /// </summary>
        public string ProductionHostRegex { get { return ConfigurationManager.AppSettings["ProductionHostRegex"]; } }
    }
}
