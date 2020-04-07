using System;
using System.IO;

namespace SignNow.Net.Test.Context
{
    public class CredentialLoader
    {
        public const string credentialsDirectory = @"Pass";

        private readonly ICredentialProvider credentialProvider;

        public CredentialLoader(Uri credentialsTarget) : this(
            new JsonFileCredentialProvider(GetCredentialsFilePath(credentialsTarget)))
        {
        }

        public CredentialLoader(ICredentialProvider credentialProvider)
        {
            this.credentialProvider = credentialProvider;
        }

        public CredentialModel GetCredentials()
        {
            return credentialProvider.GetCredential();
        }

        static string GetCredentialsFileName(Uri credentialsTarget)
        {
            return $"{credentialsTarget.Host}.json";
        }

        static string GetCredentialsFilePath(Uri credentialsTarget)
        {
            var userHomeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            return Environment.ExpandEnvironmentVariables(
                Path.Combine(userHomeFolder, credentialsDirectory, GetCredentialsFileName(credentialsTarget)));
        }
    }
}
