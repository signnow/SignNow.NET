using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SignNow.Net.Test.Context
{
    class CredentialLoader
    {
        const string credentialsDirectory = @"%USERPROFILE%\Pass";
        private readonly ICredentialProvider credentialProvider;

        public CredentialLoader(Uri credentialsTarget):this(
            new JsonFileCredentialProvider(Environment.ExpandEnvironmentVariables(Path.Combine(credentialsDirectory, credentialsTarget.Host))))
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
    }
}
