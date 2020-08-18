using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FieldInvitesTest
    {
        [DataTestMethod]
        [DataRow(InviteStatus.Created,   DisplayName = "Status: created")]
        [DataRow(InviteStatus.Pending,   DisplayName = "Status: pending")]
        [DataRow(InviteStatus.Fulfilled, DisplayName = "Status: fulfilled")]
        [DataRow(InviteStatus.Skipped,   DisplayName = "Status: skipped")]
        public void ShouldDeserializeFromJson(Enum testStatus)
        {
            var fieldInviteFake = new FieldInviteFaker()
                .RuleFor(o => o.Status, testStatus)
                .Generate();

            var expected = JsonConvert.SerializeObject(fieldInviteFake, Formatting.Indented);

            var fieldInvite = JsonConvert.DeserializeObject<FieldInvite>(expected);

            Assert.That.JsonEqual(expected, fieldInvite);
        }
    }
}
