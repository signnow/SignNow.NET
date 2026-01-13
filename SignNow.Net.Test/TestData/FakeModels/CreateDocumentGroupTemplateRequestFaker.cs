using Bogus;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="CreateDocumentGroupTemplateRequest"/>
    /// </summary>
    public class CreateDocumentGroupTemplateRequestFaker : Faker<CreateDocumentGroupTemplateRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="CreateDocumentGroupTemplateRequest"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "name": "Contract Template Group",
        ///   "folder_id": "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777",
        ///   "own_as_merged": true
        /// }
        /// </code>
        /// </example>
        public CreateDocumentGroupTemplateRequestFaker()
        {
            Rules((f, o) =>
            {
                o.Name = f.Commerce.ProductName() + " Template Group";
                o.FolderId = f.Random.Hash(40);
                o.OwnAsMerged = f.Random.Bool();
            });
        }
    }
}
