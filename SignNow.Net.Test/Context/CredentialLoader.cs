using System;
using System.IO;

namespace SignNow.Net.Test.Context
{
    public class CredentialLoader
    {
        /// <summary>
        /// Directory where is located file with credentials.
        /// </summary>
        public static string CredentialsDirectory { get; set; } = "./../../../../";

        private readonly ICredentialProvider credentialProvider;

        public CredentialLoader(Uri credentialsTarget, string basePath = default) : this(
            new JsonFileCredentialProvider(GetCredentialsFilePath(credentialsTarget)))
        {
            if (!String.IsNullOrWhiteSpace(basePath))
                CredentialsDirectory = basePath.Replace('/', Path.DirectorySeparatorChar);
        }

        public CredentialLoader(ICredentialProvider credentialProvider)
        {
            this.credentialProvider = credentialProvider;
        }

        public CredentialModel GetCredentials()
        {
            return credentialProvider.GetCredential();
        }

        private static string GetCredentialsFileName(Uri credentialsTarget)
        {
            return $"{credentialsTarget.Host}.json";
        }

        private static string GetCredentialsFilePath(Uri credentialsTarget)
        {
            return Path.GetFullPath($"{CredentialsDirectory}{GetCredentialsFileName(credentialsTarget)}");
        }
    }
}
