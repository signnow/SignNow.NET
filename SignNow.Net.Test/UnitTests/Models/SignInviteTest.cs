using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class SignInviteTest
    {
        private readonly string senderEmail = "sender@signnow.com";
        private readonly string recipientEmail = "recipient@signnow.com";

        [DataTestMethod]
        [DataRow("This is Email body message content", "This is Email subject content", DisplayName = "with custom message and subject")]
        [DataRow(default, default, DisplayName = "without message and subject")]
        public void ShouldCreateFreeformInviteContent(string message, string subject)
        {
            var invite = new FreeFormInvite(recipientEmail)
            {
                Message = message,
                Subject = subject
            };

            var sender = new User
            {
                Active = true,
                Email = senderEmail,
                FirstName = "Unit",
                LastName = "Test"
            };

            var requestInvite = JObject.FromObject(invite);

            requestInvite.Add("from", sender.Email);

            var jsonSubject = null == subject ? "null" : $"\"{subject}\"";
            var jsonMessage = null == message ? "null" : $"\"{message}\"";

            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":{jsonSubject},\"message\":{jsonMessage},\"from\":\"{senderEmail}\"}}";

            Assert.AreEqual(expected, JsonConvert.SerializeObject(requestInvite, Formatting.None));
        }

        [TestMethod]
        public void ShouldSerializeFreeFormInvite()
        {
            var ffInvite = new FreeFormInvite(recipientEmail);
            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":null,\"message\":null}}";

            Assert.AreEqual(expected, JsonConvert.SerializeObject(ffInvite));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionOnDocumentIsNull()
        {
            var invite = new RoleBasedInvite(null);
            Assert.IsNull(invite);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionOnRoleIsNull()
        {
            var invite = new RoleBasedInvite(new SignNowDocument());
            invite.AddRoleBasedInvite("email@noemail.com", null);
            Assert.IsNotNull(invite);
        }

        [TestMethod]
        public void ShouldSerializeRoleBasedInvite()
        {
            var expectedJson = @"{
                'to':[],
                'subject':null,
                'message':null
            }";

            var expected = JsonConvert.DeserializeObject(expectedJson);
            var roleBasedInvite = new RoleBasedInvite(new SignNowDocument());

            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(roleBasedInvite));
        }

        [TestMethod]
        public void ShouldCreateRoleBasedInviteContent()
        {
            var testDocJson = @"{
                'id': 'a09b26feeba7ce70228afe6290f4445700b6f349',
                'user_id': '890d13607d89a7b3f6e67a14757d02ec00cf5eae',
                'document_name': 'pdf-test',
                'page_count': '1',
                'created': '1565787561',
                'updated': '1565858757',
                'original_filename': 'pdf-test.pdf',
                'origin_user_id': null,
                'origin_document_id': null,
                'owner': 'test.dotnet@signnow.com',
                'template': false,
                'roles': [
                    {
                        'unique_id': '485a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '1',
                        'name': 'Signer 1'
                    },
                    {
                        'unique_id': '585a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '2',
                        'name': 'Signer 2'
                    }
                ],
                'requests': []
            }";

            var sender = new User
            {
                Active = true,
                Email = senderEmail,
                FirstName = "Unit",
                LastName = "Test"
            };

            var roleBasedInvite = new RoleBasedInvite(JsonConvert.DeserializeObject<SignNowDocument>(testDocJson));
            // Set user to documents' role
            var roles = roleBasedInvite.DocumentRoles();

            Assert.AreEqual(2, roles.Count);

            roleBasedInvite.AddRoleBasedInvite("signer1@signnow.com", roles.First());
            roleBasedInvite.AddRoleBasedInvite("signer2@signnow.com", roles.Last());

            // Inject sender
            var requestInvite = JObject.FromObject(roleBasedInvite);

            requestInvite.Add("from", sender.Email);
            var expectedJson = @"{
                'to':[
                    {
                        'email':'signer1@signnow.com',
                        'role':'Signer 1',
                        'role_id':'485a05488fb971644978d3ec943ff6c719bda83a',
                        'order':1
                    },
                    {
                        'email':'signer2@signnow.com',
                        'role':'Signer 2',
                        'role_id':'585a05488fb971644978d3ec943ff6c719bda83a',
                        'order':2
                    }
                ],
                'subject':null,
                'message':null,
                'from':'sender@signnow.com'
            }";

            var expected = JsonConvert.DeserializeObject(expectedJson);
            var actual = JsonConvert.SerializeObject(requestInvite, Formatting.None);

            Assert.AreEqual(JsonConvert.SerializeObject(expected, Formatting.None), actual);
        }

        [TestMethod]
        [ExpectedException(typeof(SignNowException))]
        public void ThrowsExceptionForNonExistingRole()
        {
            var testDocJson = @"{
                'id': 'a09b26feeba7ce70228afe6290f4445700b6f349',
                'user_id': '890d13607d89a7b3f6e67a14757d02ec00cf5eae',
                'document_name': 'pdf-test',
                'page_count': '1',
                'created': '1565787561',
                'updated': '1565858757',
                'original_filename': 'pdf-test.pdf',
                'origin_user_id': null,
                'origin_document_id': null,
                'owner': 'test.dotnet@signnow.com',
                'template': false,
                'roles': [
                    {
                        'unique_id': '485a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '1',
                        'name': 'Signer 1'
                    },
                    {
                        'unique_id': '585a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '2',
                        'name': 'Signer 2'
                    }
                ],
                'requests': []
            }";

            var roleInvite = new RoleBasedInvite(JsonConvert.DeserializeObject<SignNowDocument>(testDocJson));
            var fakeRole = new Role
                {
                    Id = "1234567890",
                    Name = "CEO",
                    SigningOrder = 42
                };

            roleInvite.AddRoleBasedInvite("ceo.test@signnow.com", fakeRole);
        }
    }
}
