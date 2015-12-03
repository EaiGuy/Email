namespace EaiGuy.Email.Sender
{
    using System;
    using ServiceControl.Plugin.CustomChecks;
    using log4net;

    public class SmtpConnectivityPeriodicCheck : PeriodicCheck
    {
        private readonly ILog log;

        public SmtpConnectivityPeriodicCheck()
            : base("SMTP connectivity periodic check", "Network", TimeSpan.FromMinutes(new EmailSenderSettings().PeriodCheckMinutes))
        {
            this.log = LogManager.GetLogger(GetType());
        }

        public override CheckResult PerformCheck()
        {
            try
            {
                var settings = new EmailSenderSettings();
                var emailSettings = new EmailSettings();

                if (!settings.DisablePeriodicChecks && !SmtpHelpers.TestConnection(emailSettings.SmtpServer, 25))
                    return CheckResult.Failed(string.Format("SMTP connectivity periodic check failed"));

                return CheckResult.Pass;
            }
            catch (Exception e)
            {
                log.Fatal(e.Message, e);
                return CheckResult.Failed(string.Format("SMTP connectivity periodic check failed: '{0}'", e.Message));
            }
        }
    }
}
