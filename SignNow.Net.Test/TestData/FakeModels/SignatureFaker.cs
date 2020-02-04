using Bogus;
using SignNow.Net.Internal.Model;

namespace SignNow.Net.Test.FakeModels
{
    internal class SignatureFaker : Faker<Signature>
    {
        public SignatureFaker()
        {
            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.UserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.SignatureRequestId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Email, f => f.Internet.Email());
            RuleFor(obj => obj.Created, f => f.Date.Recent().ToUniversalTime());
        }
    }
}
