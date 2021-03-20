using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="FieldInvite"/>
    /// </summary>
    public class FieldInviteFaker : Faker<FieldInvite>
    {
        /// <summary>
        /// Creates new instance of <see cref="FieldInvite"/> faker object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "279f2bee6200ac33d67778566b4bafd01a589296",
        ///   "status": "Skipped",
        ///   "role": "Signer 1",
        ///   "role_id": "b6aa248f9599317e49a07148a0e5318db42ee914",
        ///   "email": "Lacy60@yahoo.com",
        ///   "created": 1580844946,
        ///   "updated": 1580841391,
        ///   "expiration_time": 1580848760,
        ///   "is_embedded": false
        /// }
        /// </code>
        /// </example>
        public FieldInviteFaker()
        {
            Rules((f, o) =>
            {
                o.Id          = f.Random.Hash(40);
                o.Status      = f.PickRandom<InviteStatus>();
                o.RoleName    = $"Signer {f.IndexFaker + 1}";
                o.RoleId      = f.Random.Hash(40);
                o.SignerEmail = f.Internet.Email();
                o.Created     = f.Date.Recent().ToUniversalTime();
                o.Updated     = f.Date.Recent().ToUniversalTime();
                o.ExpiredOn   = f.Date.Recent().ToUniversalTime();
                o.IsEmbedded  = false;
            });
        }
    }
}
