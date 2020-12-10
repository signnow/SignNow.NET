using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignNow.Net.Test.Context
{
    class JsonCredentialProvider : ICredentialProvider
    {
        private readonly string jsonString;
        public JsonCredentialProvider(string jsonString)
        {
            this.jsonString = jsonString;
        }

        public CredentialModel GetCredential()
        {
            try
            {
                return JsonConvert.DeserializeObject<CredentialModel>(jsonString);
            }
            catch (JsonSerializationException ex)
            {
                throw new InvalidOperationException("Unable to parse values provided. You have to specify json string with login and password text properties in the constructor of this class", ex);
            }
        }
    }
}
