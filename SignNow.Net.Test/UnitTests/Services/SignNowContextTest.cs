using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace UnitTests.Services
{
    [TestClass]
    public class SignNowContextTest
    {
        [TestMethod]
        public void ShouldCreateContextUsingToken()
        {
            var context = new SignNowContext(new Token());

            Assert.IsInstanceOfType(context.OAuth, typeof(OAuth2Service));
            Assert.IsInstanceOfType(context.Documents, typeof(DocumentService));
            Assert.IsInstanceOfType(context.Invites, typeof(UserService));
            Assert.IsInstanceOfType(context.Users, typeof(UserService));
            Assert.IsInstanceOfType(context.Folders, typeof(FolderService));
            Assert.IsInstanceOfType(context.DocumentGroup, typeof(DocumentGroupService));
        }

        [TestMethod]
        public void CanSetAppCredentials()
        {
            var context = new SignNowContext(null);

            Assert.IsTrue(String.Empty.Equals(context.OAuth.ClientId));
            Assert.IsTrue(String.Empty.Equals(context.OAuth.ClientSecret));

            context.OAuth.ClientId = "CLIENT_ID";
            context.OAuth.ClientSecret = "CLIENT_SECRET";

            Assert.AreEqual("CLIENT_ID", context.OAuth.ClientId);
            Assert.AreEqual("CLIENT_SECRET", context.OAuth.ClientSecret);
        }
    }
}
