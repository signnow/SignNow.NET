using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class FreeformInviteFaker : Faker<FreeformInvite>
    {
        public FreeformInviteFaker()
        {
            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.UserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.SignatureId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Created, f => f.Date.Recent().ToUniversalTime());
            RuleFor(obj => obj.Owner, f => f.Internet.Email());
            RuleFor(obj => obj.Signer, f => f.Internet.Email());
            RuleFor(obj => obj.IsCanceled, f => f.Random.Bool());
        }
    }
}
