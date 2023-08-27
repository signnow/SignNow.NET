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
        /// {
        ///   "id": "edee8899ec00e5aaf065fb918a04f0cbc3920fac",
        ///   "origin_document_id": "ec79fd20c7dee4133d5e749e053ac8af6a63401a",
        ///   "user_id": "8f882dc816e2ab59288a9744dd84dee3ba67c0b3",
        ///   "origin_user_id": "9b96975a4c0b7701b4b547a37a7f5ed2d51edda5",
        ///   "document_name": "licensed_frozen_pants_checking_account.pdf",
        ///   "original_filename": "licensed_frozen_pants_checking_account.pdf",
        ///   "page_count": 6,
        ///   "created": "1580771931",
        ///   "updated": "1580849001",
        ///   "owner": "Richie.Dickens60@gmail.com",
        ///   "owner_name": "Richie Dickens",
        ///   "template": true,
        ///   "thumbnail": {
        ///     "small": "https://via.placeholder.com/85x110/cccccc/9c9c9c.png?text=signNow%20test",
        ///     "medium": "https://via.placeholder.com/340x440/cccccc/9c9c9c.png?text=signNow%20test",
        ///     "large": "https://via.placeholder.com/890x1151/cccccc/9c9c9c.png?text=signNow%20test"
        ///   },
        ///   "roles": [],
        ///   "signatures": [],
        ///   "fields": [],
        ///   "requests": [],
        ///   "field_invites": [],
        ///   "texts": [],
        ///   "hyperlinks": [],
        ///   "checks": [],
        ///   "attachments": [],
        ///   "enumerations": [],
        ///   "radiobuttons: []
        /// }
        /// </code>
        /// </example>
        public SignNowDocumentFaker()
        {
            Rules((f, o) =>
            {
                var filename = f.System.FileName("pdf");

                o.Id = f.Random.Hash(40);
                o.OriginDocumentId = f.Random.Hash(40);
                o.UserId = f.Random.Hash(40);
                o.OriginUserId = f.Random.Hash(40);
                o.Name = filename;
                o.OriginalName = filename;
                o.PageCount = f.Random.Int(1, 10);
                o.Created = f.Date.Recent().ToUniversalTime();
                o.Updated = f.Date.Recent().ToUniversalTime();
                o.Owner = f.Internet.Email();
                o.OwnerName = f.Person.FullName;
                o.IsTemplate = f.Random.Bool();
                o.Thumbnail = new ThumbnailFaker();
            });
        }
    }
}
