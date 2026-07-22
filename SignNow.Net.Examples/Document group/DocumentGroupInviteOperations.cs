using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Examples
{
    public partial class DocumentGroupOperations
    {
        [TestMethod]
        public async Task SendAndManageDocumentGroupInviteAsync()
        {
            // Upload two documents with signature fields so each has a role to invite
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            var documents = new List<SignNowDocument>();
            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentWithFieldExtractAsync(fileStream, $"ForDocumentGroupInviteFile-{i}.pdf")
                    .ConfigureAwait(false);
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("SendDocumentGroupInviteTest", documents)
                .ConfigureAwait(false);

            // Send a signing invite to the document group, one signer per document
            var request = new CreateGroupInviteRequest
            {
                InviteSteps = new List<GroupInviteStep>
                {
                    new GroupInviteStep
                    {
                        Order = 1,
                        InviteEmails = new List<GroupInviteEmail>
                        {
                            new GroupInviteEmail
                            {
                                Email = "signer1@signnow.com",
                                Role = documents[0].Roles[0].Name,
                                RoleId = documents[0].Roles[0].Id,
                                Order = 1
                            },
                            new GroupInviteEmail
                            {
                                Email = "signer2@signnow.com",
                                Role = documents[1].Roles[0].Name,
                                RoleId = documents[1].Roles[0].Id,
                                Order = 1
                            }
                        },
                        InviteActions = new List<GroupInviteAction>
                        {
                            new GroupInviteAction
                            {
                                Email = "signer1@signnow.com",
                                RoleName = documents[0].Roles[0].Name,
                                DocumentId = documents[0].Id
                            },
                            new GroupInviteAction
                            {
                                Email = "signer2@signnow.com",
                                RoleName = documents[1].Roles[0].Name,
                                DocumentId = documents[1].Id
                            }
                        }
                    }
                }
            };

            var invite = await testContext.GroupInvites
                .CreateGroupInviteAsync(documentGroup.Id, request)
                .ConfigureAwait(false);

            Assert.AreEqual("pending", invite.Data.Status);
            Console.WriteLine("Group invite created: {0} with status {1}", invite.Data.Id, invite.Data.Status);

            // Check the status of the group invite
            var inviteStatus = await testContext.GroupInvites
                .GetGroupInviteAsync(documentGroup.Id, invite.Data.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(invite.Data.Id, inviteStatus.Data.Id);

            // List signers who have not yet signed
            var pendingInvites = await testContext.GroupInvites
                .GetPendingGroupInvitesAsync(documentGroup.Id, invite.Data.Id)
                .ConfigureAwait(false);

            Console.WriteLine("Pending signers: {0}", pendingInvites.Data.Count);

            // Reassign the first invite step to a different signer
            var stepId = inviteStatus.Data.Steps.First().Id;
            await testContext.GroupInvites
                .ReassignSignerAsync(documentGroup.Id, invite.Data.Id, stepId, new ReassignSignerRequest
                {
                    NewSigner = new ReassignSignerInfo
                    {
                        Email = "reassigned-signer@signnow.com",
                        RoleName = documents[0].Roles[0].Name
                    }
                })
                .ConfigureAwait(false);

            Console.WriteLine("Reassigned invite step {0} to a new signer", stepId);

            // Resend invite emails to the current signers
            await testContext.GroupInvites
                .ResendGroupInviteAsync(documentGroup.Id, invite.Data.Id)
                .ConfigureAwait(false);

            // Cancel the group invite
            await testContext.GroupInvites
                .CancelGroupInviteAsync(documentGroup.Id, invite.Data.Id)
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
