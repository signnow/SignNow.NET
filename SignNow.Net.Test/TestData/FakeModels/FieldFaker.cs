using Bogus;
using SignNow.Net.Internal.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="Field"/>
    /// </summary>
    internal class FieldFaker : Faker<Field>
    {
        /// <summary>
        /// Creates new instance of <see cref="Field"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "role_id": "fedaea9d9bf46196f048ff646bdde8d172c9fb12",
        ///   "role": "Signer 1",
        ///   "originator": "owner@gmail.com",
        ///   "fulfiller": "signer1@gmail.com"
        /// }
        /// </code>
        /// </example>
        public FieldFaker()
        {
            RuleFor(r => r.Id, f => f.Random.Hash(40));
            RuleFor(r => r.RoleId, f => f.Random.Hash(40));
            RuleFor(r => r.RoleName, f => $"Signer {f.IndexFaker + 1}");
            RuleFor(r => r.Owner, f => f.Internet.Email());
            RuleFor(r => r.Signer, f => f.Internet.Email());
        }
    }
}
