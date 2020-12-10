using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SignNow.Net.Test.Context
{
    class JsonFileCredentialProvider : ICredentialProvider
    {
        private readonly string jsonFilePath;
        public string JsonFilePath => jsonFilePath;
        public JsonFileCredentialProvider(string jsonFilePath)
        {
            this.jsonFilePath = jsonFilePath;
        }

        public CredentialModel GetCredential()
        {
            if (!File.Exists(JsonFilePath))
            {
                throw new InvalidOperationException($"Unable to locate file {JsonFilePath}. {GetUsageHint(jsonFilePath)}");
            }

            var jsonCredProvider = new JsonCredentialProvider(File.ReadAllText(JsonFilePath));
            try
            {
                return jsonCredProvider.GetCredential();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Wrong file format: {ex.Message}. {GetUsageHint(jsonFilePath)}", ex);
            }
        }

        private static string GetUsageHint(string filePath)
        {
            return $"To use this class you have to create JSON file {filePath} "
                   + "with single object {{'login':'login string','password':'password string','client_id':'app client id','client_secret':'app client secret'}}";
        }
    }
}
