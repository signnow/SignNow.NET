using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Test.Constants;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    public partial class SignInviteTest
    {
        [TestMethod]
        public void ShouldSerializeRoleBasedInvite()
        {
            var expectedJson = @"{
                'to':[],
                'subject':null,
                'message':null,
                'cc':[]
            }";

            var document = new SignNowDocumentFaker()
                    .RuleFor(o => o.Roles, new RoleFaker().Generate(2));

            var roleBasedInvite = new RoleBasedInvite(document);

            AssertJson.AreEqual(expectedJson, roleBasedInvite);
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

            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(2));

            var roleBasedInvite = new RoleBasedInvite(document);
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
            var expectedJson = $@"{{
                'to':[
                    {{
                        'email':'signer1@signnow.com',
                        'role':'Signer 1',
                        'role_id':'{roles.First().Id}',
                        'order':1
                    }},
                    {{
                        'email':'signer2@signnow.com',
                        'role':'Signer 2',
                        'role_id':'{roles.Last().Id}',
                        'order':2,
                        'authentication_type':'password',
                        'password':'12345abc',
                        'expiration_days':15
                    }}
                ],
                'subject':null,
                'message':null,
                'cc':[],
                'from':'sender@signnow.com'
            }}";

            AssertJson.AreEqual(expectedJson, requestInvite);
        }

        [TestMethod]
        public void ShouldCreateRoleBasedInviteRequest()
        {
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(1));

            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count);
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name);

            AssertJson.AreEqual("{\"to\":[],\"subject\":null,\"message\":null,\"cc\":[]}", invite);

            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First())
            );

            var expected = $@"
            {{
                'to':[
                    {{
                        'email':'signer1@signnow.com',
                        'role':'Signer 1',
                        'role_id':'{invite.DocumentRoles().First().Id}',
                        'order':1
                    }}
                ],
                'subject':null,
                'message':null,
                'cc':[]
            }}";

            AssertJson.AreEqual(expected, invite);
        }

        [TestMethod]
        public void ThrowsExceptionForNotValidEmail()
        {
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(1));
            var exceptionMessage = string
                .Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, "not-an-email");

            var exceptionCtor1 = Assert
                .ThrowsException<ArgumentException>(
                    () => new RoleBasedInvite(document, "not-an-email"));

            var exceptionCtor2 = Assert
                .ThrowsException<ArgumentException>(
                    () => new RoleBasedInvite(document, new List<string> { senderEmail, "not-an-email" }));

            Assert.AreEqual(exceptionMessage, exceptionCtor1.Message);
            Assert.AreEqual(exceptionMessage, exceptionCtor2.Message);
        }

        [TestMethod]
        public void ThrowsExceptionForMissingRoleInDocument()
        {
            var fakeRole = new RoleFaker()
                .RuleFor(o => o.Name, "==CEO==")
                .Generate();

            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(10));

            var invite = new RoleBasedInvite(document);
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

            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(exception.ParamName, "document");
        }

        [TestMethod]
        public void ThrowsExceptionOnRoleIsNull()
        {
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(3));

            var invite = new RoleBasedInvite(document);

            var exception = Assert
                .ThrowsException<ArgumentNullException>(
                    () => invite.AddRoleBasedInvite(null));

            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(exception.ParamName, "options");
        }
    }
}
