namespace EaiGuy.Email.Sender
{
    using log4net;
    using ServiceControl.Plugin.CustomChecks;
    using System;

    public class PermissionsPeriodicCheck : PeriodicCheck
    {
        private readonly ILog log;

        public PermissionsPeriodicCheck()
            : base("Email Adapter Path permissions periodic check", "Security", TimeSpan.FromMinutes(new EmailSenderSettings().PeriodCheckMinutes))
        {
            this.log = LogManager.GetLogger(GetType());
        }

        public override CheckResult PerformCheck()
        {
            try
            {
                var settings = new EmailSenderSettings();
                var commonSettings = new EmailSettings();
                if (!settings.DisablePeriodicChecks && !String.IsNullOrWhiteSpace(commonSettings.WriteACopyOfAllEmailsToThisFolder))
                {
                    DirectoryHelpers.ValidatePath(commonSettings.WriteACopyOfAllEmailsToThisFolder, "WriteACopyOfAllEmailsToThisFolder");
                }
                return CheckResult.Pass;
            }
            catch (Exception e)
            {
                log.Fatal(e.Message, e);
                return CheckResult.Failed(string.Format("Email Sender Path permissions periodic check failed: '{0}'", e.Message));
            }
        }
    }
}
