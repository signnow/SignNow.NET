using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class DocumentHistoryResponseFaker : Faker<DocumentHistoryResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="DocumentHistoryResponse"/> faker object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///     "unique_id": "ca8933a3b17d252683dd0d947ed4291eac4ef4b0",
        ///     "document_id": "a26ac3f767caca1536b3ef4931feab9fa5c113e7",
        ///     "user_id": "2af15d116e04156611f82503647dabb24dc24134",
        ///     "email": "Landen_Hilpert@hotmail.com",
        ///     "client_app_name": "SignNow Web Application",
        ///     "ip_address": "127.0.0.1",
        ///     "event": "document_viewed",
        ///     "origin": "origin",
        ///     "version": 71,
        ///     "client_timestamp": null,
        ///     "created": "1697312800",
        ///     "field_id": null,
        ///     "element_id": null,
        ///     "json_attributes": null
        /// }
        /// </code>
        /// </example>
        public DocumentHistoryResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Email = f.Internet.Email();
                o.Created = f.Date.Recent().ToUniversalTime();
                o.ClientTimestamp = null;
                o.Event = "document_viewed";
                o.AppName = "SignNow Web Application";
                o.IpAddress = "127.0.0.1";
                o.UserId = f.Random.Hash(40);
                o.Email = f.Internet.Email();
                o.DocumentId = f.Random.Hash(40);
                o.FieldId = default;
                o.ElementId = default;
                o.JsonAttributes = default;
                o.Origin = "origin";
                o.Version = f.Random.Int(1, 100);
            });
        }
    }
}
