using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace UnitTests.Responses
{
    [TestClass]
    public class EmbeddedInviteResponseTest
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var jsonResponse = @"{
                'data': [
                    {
                        'id': '03739a736d324f9794c2e93ec7c5bda817af3f7f',
                        'email': 'signer@email.com',
                        'role_id': '33d7925969c53b55dfc2ea6d5d1fc8e788d33821',
                        'order': 1,
                        'status': 'pending'
                    },
                    {
                        'id': 'b926e550940746dda5206fa2bd8860ac7832b476',
                        'email': 'signer2@email.com',
                        'role_id': '8844a3615c875a74358cefa09424617e00909f0a',
                        'order': 1,
                        'status': 'pending'
                    }
                ]
            }";

            var response = TestUtils.DeserializeFromJson<EmbeddedInviteResponse>(jsonResponse);
            var inviteData1 = response.InviteData[0];

            Assert.AreEqual(2, response.InviteData.Count);
            Assert.AreEqual("03739a736d324f9794c2e93ec7c5bda817af3f7f", inviteData1.Id);
            Assert.AreEqual("signer@email.com", inviteData1.Email);
            Assert.AreEqual("33d7925969c53b55dfc2ea6d5d1fc8e788d33821", inviteData1.RoleId);
            Assert.AreEqual(1, inviteData1.Order);
            Assert.AreEqual(InviteStatus.Pending, inviteData1.Status);
        }
    }
}
