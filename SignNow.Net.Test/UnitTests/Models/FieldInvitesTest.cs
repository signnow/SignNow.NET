using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class FieldInvitesTest
    {
        [DataTestMethod]
        [DataRow("created", SignStatus.Created, DisplayName = "Status: created")]
        [DataRow("declined", SignStatus.Declined, DisplayName = "Status: declined")]
        [DataRow("fulfilled", SignStatus.Fulfilled, DisplayName = "Status: fulfilled")]
        [DataRow("pending", SignStatus.Pending, DisplayName = "Status: pending")]
        [DataRow("skipped", SignStatus.Skipped, DisplayName = "Status: skipped")]
        public void ShouldDeserializeFromJson(string status, Enum expected)
        {
            var json = $@"{{
                'id': 'a09b26test07ce70228afe6290f4445700b6f349',
                'status': '{status}',
                'role': 'Signer 1',
                'email': 'signer1@signnow.com',
                'created': '1579451165',
                'updated': '1579451165',
                'expiration_time': '1582043165'
            }}";

            var fieldInvite = JsonConvert.DeserializeObject<FieldInvite>(json);

            Assert.AreEqual("a09b26test07ce70228afe6290f4445700b6f349", fieldInvite.Id);
            Assert.AreEqual(expected, fieldInvite.Status);
            Assert.AreEqual("Signer 1", fieldInvite.RoleName);
            Assert.AreEqual("signer1@signnow.com", fieldInvite.SignerEmail);
            Assert.AreEqual("2020-01-19 16:26:05Z", fieldInvite.Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("2020-01-19 16:26:05Z", fieldInvite.Updated.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("2020-02-18 16:26:05Z", fieldInvite.ExpiredOn.ToString("u", CultureInfo.CurrentCulture));
        }
    }
}
