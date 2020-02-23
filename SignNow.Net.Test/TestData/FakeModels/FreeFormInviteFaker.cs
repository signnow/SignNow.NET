using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="FreeformInvite"/>
    /// </summary>
    public class FreeformInviteFaker : Faker<FreeformInvite>
    {
        /// <summary>
        /// Creates new instance of <see cref="FreeformInvite"/> faker object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "279f2bee6200ac33d67778566b4bafd01a589296",
        ///   "user_id": "b6aa248f9599317e49a07148a0e5318db42ee914",
        ///   "signature_id": "9640e7a2c6d2fca9282a6e24e3b5319a07718b34",
        ///   "created": 1580844946,
        ///   "originator_email": "Lacy60@yahoo.com",
        ///   "signer_email": "Bernadine46@yahoo.com",
        ///   "canceled": "1"
        /// }
        /// </code>
        /// </example>
        public FreeformInviteFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.UserId = f.Random.Hash(40);
                o.SignatureId = f.Random.Hash(40);
                o.Created = f.Date.Recent().ToUniversalTime();
                o.Owner = f.Internet.Email();
                o.SignerEmail = f.Internet.Email();
                o.IsCanceled = f.Random.Bool();
            });
        }
    }
}
