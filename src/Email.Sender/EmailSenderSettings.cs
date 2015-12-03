namespace EaiGuy.Email.Sender
{
    using System;
    using System.Configuration;

    class EmailSenderSettings
    {

        public int PeriodCheckMinutes
        {
            get
            {
                var periodicCheckMins = ConfigurationManager.AppSettings["PeriodicCheckMinutes"];
                // default to 30 minutes if configuration setting is missing
                return periodicCheckMins != null ? Convert.ToInt16(periodicCheckMins) : 30;
            }
        }

        public bool DisablePeriodicChecks
        {
            get
            {
                var disable = ConfigurationManager.AppSettings["DisablePeriodicChecks"];
                // only set to true if configuration setting exists and is true
                return disable != null && Boolean.Parse(disable);
            }
        }
    }
}