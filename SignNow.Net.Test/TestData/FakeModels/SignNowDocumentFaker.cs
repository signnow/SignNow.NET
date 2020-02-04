using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class SignNowDocumentFaker : Faker<SignNowDocument>
    {
        public SignNowDocumentFaker()
        {
            var filename = FakerHub.System.FileName("pdf");

            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.OriginDocumentId, f => f.Random.Hash(40));
            RuleFor(obj => obj.UserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.OriginUserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Name, filename);
            RuleFor(obj => obj.OriginalName, filename);
            RuleFor(obj => obj.PageCount, f => f.Random.Int(1, 10));
            RuleFor(obj => obj.Created, f => f.Date.Recent().ToUniversalTime());
            RuleFor(obj => obj.Updated, f => f.Date.Recent().ToUniversalTime());
            RuleFor(obj => obj.Owner, f => f.Internet.Email());
            RuleFor(obj => obj.IsTemplate, f => f.Random.Bool());
        }
    }
}
