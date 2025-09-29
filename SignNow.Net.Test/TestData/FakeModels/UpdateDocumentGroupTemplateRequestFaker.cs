using System.Collections.Generic;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="UpdateDocumentGroupTemplateRequest"/>
    /// </summary>
    public class UpdateDocumentGroupTemplateRequestFaker : Faker<UpdateDocumentGroupTemplateRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateDocumentGroupTemplateRequest"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "order": ["ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00789", "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00790"],
        ///   "template_group_name": "Updated Template Group",
        ///   "email_action_on_complete": "documents_and_attachments"
        /// }
        /// </code>
        /// </example>
        public UpdateDocumentGroupTemplateRequestFaker()
        {
            Rules((f, o) =>
            {
                o.Order = f.Make(f.Random.Int(1, 3), () => f.Random.Hash(40));
                o.TemplateGroupName = f.Commerce.ProductName() + " Template Group";
                o.EmailActionOnComplete = f.PickRandom(EmailActionsType.DocumentsAndAttachments, EmailActionsType.DocumentsAndAttachmentsOnlyToRecipients, EmailActionsType.WithoutDocumentsAndAttachments);
            });
        }
    }
}
