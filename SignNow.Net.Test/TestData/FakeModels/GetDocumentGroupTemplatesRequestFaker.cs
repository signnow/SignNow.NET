using Bogus;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Test.TestData.FakeModels
{
    /// <summary>
    /// Faker for generating fake GetDocumentGroupTemplatesRequest data
    /// </summary>
    public class GetDocumentGroupTemplatesRequestFaker : Faker<GetDocumentGroupTemplatesRequest>
    {
        public GetDocumentGroupTemplatesRequestFaker()
        {
            RuleFor(x => x.Limit, f => f.Random.Int(1, 50));
            RuleFor(x => x.Offset, f => f.Random.Int(0, 100));
        }
    }
}
