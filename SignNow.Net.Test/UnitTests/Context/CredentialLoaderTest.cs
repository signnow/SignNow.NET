using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test.Context;
using System;
using System.IO;

namespace UnitTests.Context
{
    [TestClass]
    public class CredentialLoaderTest
    {
        /// <summary>
        /// User Home Dir relative to OS
        /// </summary>
        public string usrHomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public string testDomain = @"https://api.signnow.test";
        public Uri testUrl;

        public string testCredentialFile;
        public string testCredentialPath;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testCredentialPath = Path.Combine(this.usrHomeDir, CredentialLoader.credentialsDirectory);
            this.testCredentialFile = Path.Combine(this.testCredentialPath, "api.signnow.test.json");

            this.testUrl = new Uri(this.testDomain);

            // Ensure Test Directory
            if (!Directory.Exists(this.testCredentialPath))
            {
                Directory.CreateDirectory(this.testCredentialPath);
            }

            // Ensure test demo file
            if(!File.Exists(this.testCredentialFile))
            {
                File.WriteAllText(this.testCredentialFile, "{'login':'test', 'password':'signnow'}");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // cleanup test file
            if (File.Exists(this.testCredentialFile))
            {
               File.Delete(this.testCredentialFile);
            }

            // remove test directory if it doesn't have any other files
            if (Directory.GetFiles(this.testCredentialPath).Length == 0)
            {
                Directory.Delete(this.testCredentialPath);
            }
        }

        [TestMethod]
        public void ItCanObtainProperFilePath()
        {
            var testCredentialLoader = new CredentialLoader(this.testUrl);
            var testCredentials = testCredentialLoader.GetCredentials();

            Assert.AreEqual("test", testCredentials.Login);
            Assert.AreEqual("signnow", testCredentials.Password);
        }
    }
}
