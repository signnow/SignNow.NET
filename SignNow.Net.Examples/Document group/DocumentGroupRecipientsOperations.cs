using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples
{
    public partial class DocumentGroupOperations
    {
        [TestMethod]
        public async Task GetAndUpdateDocumentGroupRecipientsAsync()
        {
            // Upload two documents with signature fields so each has a role to assign recipients to
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            var documents = new List<SignNowDocument>();
            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentWithFieldExtractAsync(fileStream, $"ForDocumentGroupRecipientsFile-{i}.pdf")
                    .ConfigureAwait(false);
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("DocumentGroupRecipientsTest", documents)
                .ConfigureAwait(false);

            // Get current recipients, expiration, reminder and signing order settings
            var recipients = await testContext.DocumentGroup
                .GetDocumentGroupRecipientsAsync(documentGroup.Id)
                .ConfigureAwait(false);

            Console.WriteLine("Document group has {0} recipient(s) configured", recipients.Data.Recipients?.Count ?? 0);

            // Assign a recipient to each document and configure expiration/reminder/order
            var updateRequest = new UpdateDocumentGroupRecipientsRequest
            {
                Recipients = new List<UpdateDocumentGroupRecipientEntry>
                {
                    new UpdateDocumentGroupRecipientEntry
                    {
                        Name = "Signer 1",
                        Email = "recipient1@signnow.com",
                        Order = 1,
                        Documents = new List<DocumentGroupRecipientDocument>
                        {
                            new DocumentGroupRecipientDocument
                            {
                                Id = documents[0].Id,
                                Role = documents[0].Roles[0].Name,
                                Action = "sign"
                            }
                        }
                    },
                    new UpdateDocumentGroupRecipientEntry
                    {
                        Name = "Signer 2",
                        Email = "recipient2@signnow.com",
                        Order = 2,
                        Documents = new List<DocumentGroupRecipientDocument>
                        {
                            new DocumentGroupRecipientDocument
                            {
                                Id = documents[1].Id,
                                Role = documents[1].Roles[0].Name,
                                Action = "sign"
                            }
                        }
                    }
                },
                GeneralExpirationDays = 30,
                OrderType = DocumentGroupOrderType.RecipientOrder
            };

            await testContext.DocumentGroup
                .UpdateDocumentGroupRecipientsAsync(documentGroup.Id, updateRequest)
                .ConfigureAwait(false);

            Console.WriteLine("Document group recipients updated successfully");

            // Clean up
            await testContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
            foreach (var document in documents)
            {
                DeleteTestDocument(document.Id);
            }
        }
    }
}
