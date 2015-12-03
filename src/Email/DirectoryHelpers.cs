namespace EaiGuy.Email
{
    using log4net;
    using System;
    using System.IO;

    public class DirectoryHelpers
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DirectoryHelpers));

        /// <summary>
        /// Try to create the path if it doesn't already exist; log and re-throw exceptions
        /// </summary>
        /// <param name="path">The path of the directory to validate</param>
        /// <param name="configSettingName">The name of the config setting holding the path of the directory to validate; for logging purposes only.</param>
        public static void ValidatePath(string path, string configSettingName)
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var serviceAccount = identity != null ? identity.Name : "UNKNOWN";
            log.InfoFormat("Service account is {0}", serviceAccount);
            if (!Directory.Exists(path))
            {
                log.WarnFormat("The {0} was not found or the account {1} did not have access to it: {2}", configSettingName, serviceAccount, path);

                try
                {
                    log.InfoFormat("Attempting to create directory.");
                    Directory.CreateDirectory(path);
                    log.InfoFormat("The service successfully created the directory: {0}", path);
                }
                catch (Exception e)
                {
                    log.ErrorFormat("The service was unable to create the directory {0}. Error: {1}", path, e.Message);
                    throw e;
                }
            }
            else
            {
                log.InfoFormat("The path {0} exists and account {1} can read", path, serviceAccount);
                // try creating then deleting a file to ensure we have write access
                log.InfoFormat("Attempting to create file.");
                string randomFileName = String.Format("BlankFile{0}.txt", Guid.NewGuid().ToString());
                string qualifiedName = Path.Combine(path, randomFileName);
                File.WriteAllText(qualifiedName, "Validating that we can write to this folder");
                File.Delete(qualifiedName);
                log.InfoFormat("Account{0} has write permissions.", serviceAccount);
            }
        }
    }
}
