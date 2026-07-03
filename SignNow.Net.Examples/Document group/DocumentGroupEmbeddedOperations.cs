using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Examples
{
    public partial class DocumentGroupOperations
    {
        [TestMethod]
        public async Task CreateAndManageDocumentGroupEmbeddedInviteAsync()
        {
            // Upload two documents with signature fields so each has a role to invite
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            var documents = new List<SignNowDocument>();
            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentWithFieldExtractAsync(fileStream, $"ForDocumentGroupEmbeddedFile-{i}.pdf")
                    .ConfigureAwait(false);
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("DocumentGroupEmbeddedInviteTest", documents)
                .ConfigureAwait(false);

            // Create an embedded signing invite for the document group, without sending emails
            var embeddedInviteRequest = new CreateDocumentGroupEmbeddedInviteRequest
            {
                Invites = new List<DocumentGroupEmbeddedInviteSigner>
                {
                    new DocumentGroupEmbeddedInviteSigner
                    {
                        Email = "embedded-signer1@signnow.com",
                        RoleId = documents[0].Roles[0].Id,
                        SigningOrder = 1
                    },
                    new DocumentGroupEmbeddedInviteSigner
                    {
                        Email = "embedded-signer2@signnow.com",
                        RoleId = documents[1].Roles[0].Id,
                        SigningOrder = 1
                    }
                }
            };

            var embeddedInvite = await testContext.DocumentGroup
                .CreateDocumentGroupEmbeddedInviteAsync(documentGroup.Id, embeddedInviteRequest)
                .ConfigureAwait(false);

            Assert.AreEqual(2, embeddedInvite.InviteData.Count);
            Console.WriteLine("Created {0} embedded invites for the document group", embeddedInvite.InviteData.Count);

            // Generate a signing link for the first embedded invite
            var embeddedInviteLink = await testContext.DocumentGroup
                .GenerateDocumentGroupEmbeddedInviteLinkAsync(
                    documentGroup.Id,
                    embeddedInvite.InviteData[0].Id,
                    new CreateDocumentGroupEmbedLinkOptions { LinkExpiration = 30 })
                .ConfigureAwait(false);

            Console.WriteLine("Embedded invite link: {0}", embeddedInviteLink.Link.AbsoluteUri);

            // Generate a link to open the embedded editor for the document group
            var editorLink = await testContext.DocumentGroup
                .GenerateDocumentGroupEmbeddedEditorLinkAsync(
                    documentGroup.Id,
                    new EmbeddedEditorOptions { LinkExpiration = 30 })
                .ConfigureAwait(false);

            Console.WriteLine("Embedded editor link: {0}", editorLink.Link.AbsoluteUri);

            // Generate a link to open the embedded sending workflow for the document group
            var sendingLink = await testContext.DocumentGroup
                .GenerateDocumentGroupEmbeddedSendingLinkAsync(
                    documentGroup.Id,
                    new EmbeddedSendingOptions { LinkExpiration = 30 })
                .ConfigureAwait(false);

            Console.WriteLine("Embedded sending link: {0}", sendingLink.Link.AbsoluteUri);

            // Cancel all embedded signing invites for the document group
            await testContext.DocumentGroup
                .CancelDocumentGroupEmbeddedInviteAsync(documentGroup.Id)
                .ConfigureAwait(false);

            // Clean up
            await testContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
            foreach (var document in documents)
            {
                DeleteTestDocument(document.Id);
            }
        }
    }
}
