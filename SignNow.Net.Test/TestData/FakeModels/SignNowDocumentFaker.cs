using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class SignNowDocumentFaker : Faker<SignNowDocument>
    {
        /// <summary>
        /// Faker <see cref="SignNowDocument"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        ///  {
        ///      "id": "edee8899ec00e5aaf065fb918a04f0cbc3920fac",
        ///      "origin_document_id": "ec79fd20c7dee4133d5e749e053ac8af6a63401a",
        ///      "user_id": "8f882dc816e2ab59288a9744dd84dee3ba67c0b3",
        ///      "origin_user_id": "9b96975a4c0b7701b4b547a37a7f5ed2d51edda5",
        ///      "document_name": "licensed_frozen_pants_checking_account.pdf",
        ///      "original_filename": "licensed_frozen_pants_checking_account.pdf",
        ///      "page_count": 6,
        ///      "created": "1580771931",
        ///      "updated": "1580849001",
        ///      "owner": "Richie.Dickens60@gmail.com",
        ///      "template": true,
        ///      "roles": [],
        ///      "signatures": [],
        ///      "requests": [],
        ///      "field_invites": []
        ///  }
        /// </code>
        /// </example>
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

    public class SignNowDocumentWithFieldsFaker : SignNowDocumentFaker
    {
        /// <summary>
        /// Faker <see cref="SignNowDocument"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        ///  {
        ///      "id": "edee8899ec00e5aaf065fb918a04f0cbc3920fac",
        ///      "origin_document_id": "ec79fd20c7dee4133d5e749e053ac8af6a63401a",
        ///      "user_id": "8f882dc816e2ab59288a9744dd84dee3ba67c0b3",
        ///      "origin_user_id": "9b96975a4c0b7701b4b547a37a7f5ed2d51edda5",
        ///      "document_name": "licensed_frozen_pants_checking_account.pdf",
        ///      "original_filename": "licensed_frozen_pants_checking_account.pdf",
        ///      "page_count": 6,
        ///      "created": "1580771931",
        ///      "updated": "1580849001",
        ///      "owner": "Richie.Dickens60@gmail.com",
        ///      "template": true,
        ///      "roles": [
        ///        {
        ///          "unique_id": "cd3421c6f5cecb68b826d63d749f87c35e915fe3",
        ///          "signing_order": 1,
        ///          "name": "Signer 1"
        ///        }
        ///      ],
        ///      "signatures": [],
        ///      "requests": [],
        ///      "field_invites": []
        ///  }
        /// </code>
        /// </example>
        public SignNowDocumentWithFieldsFaker()
        {
            RuleFor(obj => obj.Roles, new RoleFaker().Generate(1));
        }
    }
}
