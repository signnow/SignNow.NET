using System.Text;
using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="Signature"/>
    /// </summary>
    internal class SignatureFaker : Faker<Signature>
    {
        /// <summary>
        /// Creates new instance of <see cref="Signature"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "49d2b8a40be75e853f1352afe33dfbb015829039",
        ///   "user_id": "625f0b4b6f345dc3280385667bdb62f100a1344c",
        ///   "signature_request_id": "350c40cf474bdb496456608774fb4789214ec810",
        ///   "email": "Alek.Keebler19@yahoo.com",
        ///   "created": "1581765801",
        ///   "data": "dGVzdDEuLi4="
        /// }
        /// </code>
        /// </example>
        public SignatureFaker()
        {
            Rules((f, o) =>
            {
                o.Id                 = f.Random.Hash(40);
                o.UserId             = f.Random.Hash(40);
                o.SignatureRequestId = f.Random.Hash(40);
                o.Email              = f.Internet.Email();
                o.Created            = f.Date.Recent().ToUniversalTime();
                o.Data               = Encoding.UTF8.GetBytes(f.Lorem.Paragraph());
            });
        }
    }
}
