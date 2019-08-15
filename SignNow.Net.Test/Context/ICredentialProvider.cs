using System;
using System.Collections.Generic;
using System.Text;

namespace SignNow.Net.Test.Context
{
    interface ICredentialProvider
    {
        CredentialModel GetCredential();
    }
}
