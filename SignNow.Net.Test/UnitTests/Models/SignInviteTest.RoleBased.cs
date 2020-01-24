using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Test.Constants;

namespace UnitTests
{
    public partial class SignInviteTest
    {
        private readonly string testDocJson = @"{
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

        [TestMethod]
        public void ShouldSerializeRoleBasedInvite()
        {
            var expectedJson = @"{
                'to':[],
                'subject':null,
                'message':null
            }";

            var expected = JsonConvert.DeserializeObject(expectedJson);
            var roleBasedInvite = new RoleBasedInvite(JsonConvert.DeserializeObject<SignNowDocument>(testDocJson));

            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(roleBasedInvite));
        }

        [TestMethod]
        public void ShouldCreateRoleBasedInviteContent()
        {
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

            var signer1Options = new SignerOptions("signer1@signnow.com", roles.First());
            var signer2Options = new SignerOptions("signer2@signnow.com", roles.Last())
                {
                    ExpirationDays = 15
                }
                .SetAuthenticationByPassword("12345abc");

            roleBasedInvite.AddRoleBasedInvite(signer1Options);
            roleBasedInvite.AddRoleBasedInvite(signer2Options);

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
                        'order':2,
                        'authentication_type':'password',
                        'password':'12345abc',
                        'expiration_days':15
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
        public void ThrowsExceptionForMissingRoleInDocument()
        {
            var fakeRole = new Role
            {
                Id = "1",
                Name = "CEO",
                SigningOrder = 1
            };

            var invite = new RoleBasedInvite(JsonConvert.DeserializeObject<SignNowDocument>(testDocJson));
            var exception = Assert
                .ThrowsException<SignNowException>(
                    () => invite.AddRoleBasedInvite(
                        new SignerOptions("test@email.com", fakeRole)));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture,  ExceptionMessages.CannotAddRole, fakeRole.Name),
                exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionForDocumentWithoutRoles()
        {
            var exception = Assert
                .ThrowsException<ArgumentException>(
                    () => new RoleBasedInvite(new SignNowDocument()));

            Assert.AreEqual(ExceptionMessages.NoFillableFieldsWithRole, exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionOnDocumentIsNull()
        {
            var exception = Assert
                .ThrowsException<ArgumentNullException>(
                    () => new RoleBasedInvite(null));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "document"),
                exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionOnRoleIsNull()
        {
            var invite = new RoleBasedInvite(
                JsonConvert.DeserializeObject<SignNowDocument>(testDocJson));

            var exception = Assert
                .ThrowsException<ArgumentNullException>(
                    () => invite.AddRoleBasedInvite(null));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "options"),
                exception.Message);
        }
    }
}
