using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="EmbeddedInvite"/>
    /// </summary>
    public class EmbeddedInviteFaker : Faker<EmbeddedInvite>
    {
        /// <summary>
        /// Faker <see cref="EmbeddedInvite"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "email": "Jesus_Monahan@gmail.com",
        ///   "auth_method": "none,
        ///   "role_id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "order": 1
        /// }
        /// </code>
        /// </example>
        public EmbeddedInviteFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.AuthMethod = "none";
                o.RoleId = f.Random.Hash(40);
                o.SigningOrder = (uint)f.Random.Number(1, 10);
            });
        }
    }
}
