using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Models
{
    [TestClass]
    public class RoleContentTest
    {
        private Role role { get; set; }

        [TestInitialize]
        public void Setup()
        {
            role = new RoleFaker().Generate();
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithPasswordAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 14,
                    RemindAfterDays = 7
                }
                .SetAuthenticationByPassword("***secret***");

            var expected = $@"{{
                'email': 'test@email.com',
                'role': '{role.Name}',
                'role_id': '{role.Id}',
                'order': {role.SigningOrder},
                'authentication_type': 'password',
                'password': '***secret***',
                'expiration_days': 14,
                'reminder': 7
            }}";

            Assert.That.JsonEqual(expected, content);
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithPhoneAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 7
                }
                .SetAuthenticationByPhoneCall("800 831-2050");

            var expected = $@"{{
                'email': 'test@email.com',
                'role': '{role.Name}',
                'role_id': '{role.Id}',
                'order': {role.SigningOrder},
                'authentication_type': 'phone_call',
                'phone': '800 831-2050',
                'expiration_days': 7
            }}";

            Assert.That.JsonEqual(expected, content);
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithSmsAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    RemindAfterDays = 1
                }
                .SetAuthenticationBySms("800 831-2050");

            var expected = $@"{{
                'email': 'test@email.com',
                'role': '{role.Name}',
                'role_id': '{role.Id}',
                'order': {role.SigningOrder},
                'authentication_type': 'sms',
                'phone': '800 831-2050',
                'reminder': 1
            }}";

            Assert.That.JsonEqual(expected, content);
        }

        [TestMethod]
        public void ShouldSetOnlyOneProtection()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 5,
                    RemindAfterDays = 1
                };

            content.SetAuthenticationBySms("800 831-2050");
            Assert.AreEqual("800 831-2050", content.SignerAuth.Phone);
            Assert.AreEqual("sms", content.SignerAuth.AuthenticationType);
            Assert.IsNull(content.SignerAuth.Password);

            content.SetAuthenticationByPassword("secret");
            Assert.AreEqual("password", content.SignerAuth.AuthenticationType);
            Assert.AreEqual("secret", content.SignerAuth.Password);
            Assert.IsNull(content.SignerAuth.Phone);

            content.SetAuthenticationByPhoneCall("800 831-2050");
            Assert.AreEqual("800 831-2050", content.SignerAuth.Phone);
            Assert.AreEqual("phone_call", content.SignerAuth.AuthenticationType);
            Assert.IsNull(content.SignerAuth.Password);

            content.ClearSignerAuthentication();
            Assert.IsNull(content.SignerAuth?.Password);
            Assert.IsNull(content.SignerAuth?.Phone);
            Assert.IsNull(content.SignerAuth?.AuthenticationType);
        }
    }
}
