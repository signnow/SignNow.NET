using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="FieldInvite">
    /// </summary>
    public class FieldInviteFaker : Faker<FieldInvite>
    {
        /// <summary>
        /// Creates new instance of <see cref="FieldInvite"> faker object.
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
        ///   "expiration_time": 1580848760
        /// }
        /// </code>
        /// </example>
        public FieldInviteFaker()
        {
            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.Status, f => f.PickRandom<FieldInvitesStatus>());
            RuleFor(obj => obj.RoleName, f => $"Signer {f.IndexFaker + 1}");
            RuleFor(obj => obj.RoleId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Email, f => f.Internet.Email());
            RuleFor(obj => obj.Created, f => f.Date.Recent().ToUniversalTime());
            RuleFor(obj => obj.Updated, f => f.Date.Recent().ToUniversalTime());
            RuleFor(obj => obj.ExpiredOn, f => f.Date.Recent().ToUniversalTime());
        }
    }
}
