using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class RoleContentTest
    {
        private Role role { get; set; }

        [TestInitialize]
        public void Setup()
        {
            role = new Role
            {
                Id = "12345abc",
                Name = "Signer 1",
                SigningOrder = 1
            };
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithPasswordAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 14,
                    RemindAfterDays = 7
                }
                .SetProtectionByPassword("***secret***");

            var json = JsonConvert.DeserializeObject(@"{
                'email': 'test@email.com',
                'role': 'Signer 1',
                'role_id': '12345abc',
                'order': 1,
                'authentication_type': 'password',
                'password': '***secret***',
                'expiration_days': 14,
                'reminder': 7
            }");

            var actual = JsonConvert.SerializeObject(content, Formatting.Indented);
            var expected = JsonConvert.SerializeObject(json, Formatting.Indented);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithPhoneAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 7
                }
                .SetProtectionByPhoneCall("800 831-2050");

            var json = JsonConvert.DeserializeObject(@"{
                'email': 'test@email.com',
                'role': 'Signer 1',
                'role_id': '12345abc',
                'order': 1,
                'authentication_type': 'phone_call',
                'phone': '800 831-2050',
                'expiration_days': 7
            }");

            var actual = JsonConvert.SerializeObject(content, Formatting.Indented);
            var expected = JsonConvert.SerializeObject(json, Formatting.Indented);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldCreateRoleContentWithSmsAccess()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    RemindAfterDays = 1
                }
                .SetProtectionBySms("800 831-2050");

            var json = JsonConvert.DeserializeObject(@"{
                'email': 'test@email.com',
                'role': 'Signer 1',
                'role_id': '12345abc',
                'order': 1,
                'authentication_type': 'sms',
                'phone': '800 831-2050',
                'reminder': 1
            }");

            var actual = JsonConvert.SerializeObject(content, Formatting.Indented);
            var expected = JsonConvert.SerializeObject(json, Formatting.Indented);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldSetOnlyOneProtection()
        {
            var content = new SignerOptions("test@email.com", role)
                {
                    ExpirationDays = 5,
                    RemindAfterDays = 1
                };

            content.SetProtectionBySms("800 831-2050");
            Assert.AreEqual("800 831-2050", content.Phone);
            Assert.AreEqual("sms", content.AuthenticationType);
            Assert.IsNull(content.Password);

            content.SetProtectionByPassword("secret");
            Assert.AreEqual("password", content.AuthenticationType);
            Assert.AreEqual("secret", content.Password);
            Assert.IsNull(content.Phone);

            content.SetProtectionByPhoneCall("800 831-2050");
            Assert.AreEqual("800 831-2050", content.Phone);
            Assert.AreEqual("phone_call", content.AuthenticationType);
            Assert.IsNull(content.Password);

            content.ClearSignerAccessProtection();
            Assert.IsNull(content.Password);
            Assert.IsNull(content.Phone);
            Assert.IsNull(content.AuthenticationType);
        }
    }
}
