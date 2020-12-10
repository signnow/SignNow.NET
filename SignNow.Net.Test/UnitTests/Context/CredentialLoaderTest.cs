using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test.Context;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class CredentialLoaderTest
    {
        private readonly string testDomain = @"https://api.signnow.test";
        private Uri testUrl;

        private string testCredentialFile;
        private string testCredentialPath;

        [TestInitialize]
        public void Setup()
        {
            testCredentialPath = Path.GetFullPath(CredentialLoader.CredentialsDirectory);
            testCredentialFile = Path.GetFullPath($"{testCredentialPath}api.signnow.test.json");

            testUrl = new Uri(testDomain);

            // Ensure Test Directory
            if (!Directory.Exists(testCredentialPath))
            {
                Directory.CreateDirectory(testCredentialPath);
            }

            // Ensure test demo file
            if (!File.Exists(testCredentialFile))
            {
                File.WriteAllText(
                    testCredentialFile,
                    "{'login':'test', 'password':'signnow', 'client_id':'test001', 'client_secret':'test002'}");
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            // cleanup test file
            if (File.Exists(testCredentialFile))
            {
               File.Delete(testCredentialFile);
            }

            // remove test directory if it doesn't have any other files
            if (Directory.GetFiles(testCredentialPath).Length == 0)
            {
                Directory.Delete(testCredentialPath);
            }
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Globalization", "CA1305:string.Format could vary based on locale", Justification = "Locale is not used for this test")]
        public void ItCanObtainProperFilePath()
        {
            var TestCreds = new CredentialLoader(testUrl).GetCredentials();

            const string message = "You have to specify json string with `{0}` text properties in the {1}";

            Assert.AreEqual("test", TestCreds.Login, string.Format(message, "login", testCredentialFile));
            Assert.AreEqual("signnow", TestCreds.Password, string.Format(message, "password", testCredentialFile));
            Assert.AreEqual("test001", TestCreds.ClientId, string.Format(message, "client_id", testCredentialFile));
            Assert.AreEqual("test002", TestCreds.ClientSecret, string.Format(message, "client_secret", testCredentialFile));
        }

        [TestMethod]
        public void ShowHintWhenCredentialsFileNotExists()
        {
            var Credentials = new CredentialLoader(new Uri("https://api.signnow.local"));

            var exception = Assert.ThrowsException<InvalidOperationException>(
                () => Credentials.GetCredentials());

            StringAssert.Contains(exception.Message, "api.signnow.local.json");
            StringAssert.Contains(exception.Message, "{{'login':'login string','password':'password string','client_id':'app client id','client_secret':'app client secret'}}");
        }
    }
}
