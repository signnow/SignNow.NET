using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace UnitTests
{
    [TestClass]
    public class SignNowContextTest
    {
        [TestMethod]
        public void ShouldCreateContextUsingToken()
        {
            var context = new SignNowContext(new Token());

            Assert.IsInstanceOfType(context.Documents, typeof(DocumentService));
            Assert.IsInstanceOfType(context.Invites, typeof(UserService));
            Assert.IsInstanceOfType(context.Users, typeof(UserService));
        }
    }
}
