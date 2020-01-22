using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace UnitTests
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void ThrowsExceptionOnInviteIsNull()
        {
            var service = new UserService(new Token());
            var response = service.CancelInviteAsync(null as FreeformInvite).Exception;

            Assert.IsNotNull(response);
            StringAssert.Contains(response.InnerException?.Message, "Parameter name: invite");
        }
    }
}
