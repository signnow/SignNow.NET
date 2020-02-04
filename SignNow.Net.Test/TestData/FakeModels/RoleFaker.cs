using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class RoleFaker : Faker<Role>
    {
        public RoleFaker()
        {
            FakerHub.IndexVariable = ++FakerHub.IndexFaker + 1;

            RuleFor(r => r.Id, f => f.Random.Hash(40));
            RuleFor(r => r.Name, f => $"Signer {f.IndexVariable}");
            RuleFor(r => r.SigningOrder, f => f.IndexVariable);
        }
    }
}
