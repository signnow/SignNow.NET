using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="Role"/>
    /// </summary>
    public class RoleFaker : Faker<Role>
    {
        /// <summary>
        /// Creates new instance of <see cref="Role"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "unique_id": "cd3421c6f5cecb68b826d63d749f87c35e915fe3",
        ///   "signing_order": 1,
        ///   "name": "Signer 1"
        /// }
        /// </code>
        /// </example>
        public RoleFaker()
        {
            RuleFor(r => r.Id, f => f.Random.Hash(40));
            RuleFor(r => r.Name, f => $"Signer {f.IndexFaker + 1}");
            RuleFor(r => r.SigningOrder, f => f.IndexFaker + 1);
        }
    }
}
