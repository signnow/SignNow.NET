using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Examples.Documents;
using SignNow.Net.Examples.Folders;
using SignNow.Net.Examples.Invites;
using SignNow.Net.Examples.Users;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.ComplexTags;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.EventSubscriptionBase;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Test.Context;
using UnitTests;

namespace SignNow.Net.Examples
{
    /// <summary>
    /// This Test class contains all tests for Code Samples.
    ///
    /// To run single test from console:
    ///     # For example we want to run only RequestAccessTokenTest
    ///     dotnet test SignNow.Net.Examples --filter RequestAccessTokenTest
    /// </summary>
    [TestClass]
    public abstract class ExamplesRunner
    {
        private DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Base path to the `TestExamples` directory.
        /// Path should use Unix-like directory separator char. It requires for cross-platform path compatibility.
        /// </summary>
        private static readonly string BaseTestExamplesPath = "../../../TestExamples/"
            .Replace('/', Path.DirectorySeparatorChar);

        protected static readonly string PdfWithSignatureField = Path.Combine(BaseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");
        protected static readonly string PdfWithoutFields = Path.Combine(BaseTestExamplesPath, "SignAndDate.pdf");
        protected static readonly string PdfWithComplexTags = Path.Combine(BaseTestExamplesPath, "ComplexTags.pdf");

        /// <summary>
        /// Contains application clientId, clientSecret and user credentials
        /// </summary>
        protected static CredentialModel credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();

        /// <summary>
        /// signNow service container used for ExampleRunner
        /// </summary>
        protected static SignNowContext testContext;

        /// <summary>
        /// signNow API base Url (sandbox)
        /// </summary>
        public static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");

        /// <summary>
        /// Delete test document after test.
        /// </summary>
        protected void DeleteTestDocument(string disposableDocumentId)
        {
            if (string.IsNullOrEmpty(disposableDocumentId))
            {
                return;
            }

            var documentTask = testContext.Documents
                .DeleteDocumentAsync(disposableDocumentId);

            Task.WaitAll(documentTask);

            Assert.IsFalse(documentTask.IsFaulted);
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // If you want to use your own credentials just for simple and fast test
            // uncomment next lines bellow and replace placeholders with your credentials:

            // credentials = new CredentialModel
            // {
            //     Login = "user_eamail@noemail.com",
            //     Password = "your-secret-password",
            //     ClientId = "your-application-client-id",
            //     ClientSecret = "your-application-client-secret"
            // };

            var client = new HttpClient();

            // Create signNow context with all the services and Authorization
            testContext = new SignNowContext(ApiBaseUrl, null, client);
            testContext.SetAppCredentials(credentials.ClientId, credentials.ClientSecret);
            testContext.GetAccessToken(credentials.Login, credentials.Password, Scope.All);
        }



        // #region Invites Examples
        //
        // /// <summary>
        // /// Run test for example: <see cref="InviteExamples.CreateFreeformInviteToSignTheDocument"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateFreeformInviteToSignTheDocumentTest()
        // {
        //     await using var fileStream = File.OpenRead(PdfWithoutFields);
        //     var document = await testContext.Documents
        //         .UploadDocumentAsync(fileStream, "CreateFreeformInviteToSignTheDocument.pdf")
        //         .ConfigureAwait(false);
        //
        //     var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //     Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);
        //
        //     var inviteResponse = await InviteExamples
        //         .CreateFreeformInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsFalse(string.IsNullOrEmpty(inviteResponse.Id));
        //
        //     var documentWithInvite = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //     var createdInvite = documentWithInvite.InvitesStatus.FirstOrDefault();
        //
        //     Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
        //     Assert.AreEqual(inviteResponse.Id, createdInvite?.Id);
        //     Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
        //     Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="InviteExamples.CreateRoleBasedInviteToSignTheDocument"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateRoleBasedInviteToSignTheDocumentTest()
        // {
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //     var document = await testContext.Documents
        //         .UploadDocumentWithFieldExtractAsync(fileStream, "CreateRoleBasedInviteToSignTheDocument.pdf")
        //         .ConfigureAwait(false);
        //
        //     var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //     Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);
        //
        //     var inviteResponse = await InviteExamples
        //         .CreateRoleBasedInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");
        //
        //     var documentWithInvite = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //     var createdInvite = documentWithInvite.FieldInvites.FirstOrDefault();
        //
        //     var fieldInvite = documentWithInvite.Fields.FirstOrDefault();
        //     Assert.IsNotNull(fieldInvite?.FieldRequestId);
        //
        //     await testContext.Invites
        //         .ResendEmailInviteAsync(fieldInvite?.FieldRequestId)
        //         .ConfigureAwait(false);
        //
        //     Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
        //     Assert.AreEqual("Signer 1", createdInvite?.RoleName, "Signer role mismatch.");
        //     Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
        //     Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);
        // }
        //
        // /// <summary>
        // /// Run test for example:
        // /// <see cref="InviteExamples.CreateEmbeddedSigningInviteToSignTheDocument"/>
        // /// <see cref="InviteExamples.GenerateLinkForEmbeddedInvite"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateEmbeddedSigningInviteToSignTheDocumentTest()
        // {
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //     var document = await testContext.Documents
        //         .UploadDocumentWithFieldExtractAsync(fileStream, "CreateEmbeddedSigningInviteToSignTheDocument.pdf")
        //         .ConfigureAwait(false);
        //
        //     var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //
        //     // Create Embedded Signing Invite
        //     var embeddedInviteResponse = await InviteExamples
        //         .CreateEmbeddedSigningInviteToSignTheDocument(signNowDoc, "testemail@signnow.com", testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.AreEqual(1, embeddedInviteResponse.InviteData.Count);
        //     Assert.AreEqual(1, embeddedInviteResponse.InviteData[0].Order);
        //     Assert.AreEqual("testemail@signnow.com", embeddedInviteResponse.InviteData[0].Email);
        //     Assert.AreEqual("Pending", embeddedInviteResponse.InviteData[0].Status.ToString());
        //
        //
        //     var documentWithEmbed = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
        //     Assert.IsTrue(documentWithEmbed.FieldInvites.First().IsEmbedded);
        //
        //     // Generate link for Embedded Signing Invite
        //     var embeddedLink = await InviteExamples
        //         .GenerateLinkForEmbeddedInvite(documentWithEmbed, 30, testContext).ConfigureAwait(false);
        //
        //     Assert.IsInstanceOfType(embeddedLink.Link, typeof(Uri));
        //     Console.WriteLine($@"Embedded link: {embeddedLink.Link.AbsoluteUri}");
        //
        //     // Cancel embedded invite
        //     await InviteExamples.CancelEmbeddedInvite(documentWithEmbed, testContext).ConfigureAwait(false);
        //     DeleteTestDocument(document.Id);
        //     DeleteTestDocument(documentWithEmbed.Id);
        // }
        //
        // #endregion
        //
        // #region User Examples
        //
        // /// <summary>
        // /// Run test for <see cref="UserExamples.CreateSignNowUser"/> and <see cref="UserExamples.SendVerificationEmailToUser"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateSignNowUserTest()
        // {
        //     var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;
        //
        //     var createUserResponse = await UserExamples.CreateSignNowUser(
        //         "John",
        //         $"Sample{timestamp}",
        //         $"signnow.tutorial+sample_test{timestamp}@gmail.com",
        //         "secretPassword",
        //         testContext
        //     ).ConfigureAwait(false);
        //
        //     Assert.AreEqual($"signnow.tutorial+sample_test{timestamp}@gmail.com", createUserResponse.Email);
        //     Assert.IsFalse(createUserResponse.Verified);
        //
        //     // Finally - send verification email to User
        //     await UserExamples.SendVerificationEmailToUser(createUserResponse.Email, testContext).ConfigureAwait(false);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="UserExamples.GetUserModifiedDocuments"/>
        // /// </summary>
        // [TestMethod]
        // public async Task GetUserModifiedDocumentsTest()
        // {
        //     var perPage = 25;
        //     var SignNowDocumentsAsync = await UserExamples
        //         .GetUserModifiedDocuments(perPage, testContext)
        //         .ConfigureAwait(false);
        //
        //     var modifiedDocuments = SignNowDocumentsAsync.ToList();
        //     foreach (var document in modifiedDocuments)
        //     {
        //         Assert.AreEqual(credentials.Login, document.Owner);
        //     }
        //
        //     Assert.IsNotNull(modifiedDocuments.Count);
        //     Console.WriteLine($@"Total modified documents: {modifiedDocuments.Count}");
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="UserExamples.GetUserDocuments"/>
        // /// </summary>
        // [TestMethod]
        // public async Task GetUserDocumentsTest()
        // {
        //     var perPage = 25;
        //     var SignNowDocumentsAsync = await UserExamples
        //         .GetUserDocuments(perPage, testContext)
        //         .ConfigureAwait(false);
        //
        //     var userDocuments = SignNowDocumentsAsync.ToList();
        //     foreach (var document in userDocuments)
        //     {
        //         Assert.AreEqual(credentials.Login, document.Owner);
        //     }
        //
        //     Assert.IsNotNull(userDocuments.Count);
        //     Console.WriteLine($@"Total modified documents: {userDocuments.Count}");
        // }
        //
        // #endregion
        //
        // #region Templates Examples
        //
        // /// <summary>
        // /// Run test for example: <see cref="DocumentExamples.CreateTemplateFromTheDocument"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateTemplateFromDocumentTest()
        // {
        //     var document = await DocumentExamples
        //         .UploadDocumentWithFieldExtract(PdfWithSignatureField, testContext).ConfigureAwait(false);
        //
        //     const string templateName = "Template Name";
        //     var result = await DocumentExamples.CreateTemplateFromTheDocument(document?.Id, templateName, testContext).ConfigureAwait(false);
        //     var template = await testContext.Documents.GetDocumentAsync(result.Id).ConfigureAwait(false);
        //
        //     Assert.IsFalse(document?.IsTemplate);
        //     Assert.IsNotNull(template?.Id);
        //     Assert.AreEqual(templateName, template.Name);
        //     Assert.IsTrue(template.IsTemplate);
        //
        //     await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
        //     DeleteTestDocument(document?.Id);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="DocumentExamples.CreateDocumentFromTheTemplate"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateDocumentFromTemplateTest()
        // {
        //     var testDocument = await DocumentExamples
        //         .UploadDocumentWithFieldExtract(PdfWithSignatureField, testContext)
        //         .ConfigureAwait(false);
        //
        //     var template = await testContext.Documents
        //         .CreateTemplateFromDocumentAsync(testDocument.Id, "TemplateName")
        //         .ConfigureAwait(false);
        //     var documentName = "Document Name";
        //     var result = await DocumentExamples
        //         .CreateDocumentFromTheTemplate(template.Id, documentName, testContext)
        //         .ConfigureAwait(false);
        //     var document = await testContext.Documents
        //         .GetDocumentAsync(result.Id)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsNotNull(document?.Id);
        //     Assert.IsFalse(document.IsTemplate);
        //     Assert.AreEqual(documentName, document.Name);
        //
        //     await testContext.Documents.DeleteDocumentAsync(document.Id).ConfigureAwait(false);
        //     await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
        //     DeleteTestDocument(testDocument?.Id);
        // }
        //
        // #endregion
        //
        // #region Folder Examples
        //
        // /// <summary>
        // /// Run test for example: <see cref="FolderExamples.GetAllFolders"/>
        // /// </summary>
        // [TestMethod]
        // public async Task GetAllFoldersTest()
        // {
        //     var folders = await FolderExamples
        //         .GetAllFolders(testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsInstanceOfType(folders, typeof(SignNowFolders));
        //     Assert.AreEqual("Root", folders.Name);
        //     Assert.IsTrue(folders.SystemFolder);
        //
        //     Assert.IsTrue(folders.Folders.Any(f => f.Name == "Documents"));
        //     Assert.IsTrue(folders.Folders.Any(f => f.Name == "Archive"));
        //     Assert.IsTrue(folders.Folders.Any(f => f.Name == "Templates"));
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="FolderExamples.GetFolder"/>
        // /// </summary>
        // [TestMethod]
        // public async Task GetFolderTest()
        // {
        //     var folders = await FolderExamples.GetAllFolders(testContext).ConfigureAwait(false);
        //     var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;
        //
        //     var filterBySigningStatus = new GetFolderOptions
        //     {
        //         Filters = new FolderFilters(SigningStatus.Pending)
        //     };
        //
        //     var folder = await FolderExamples
        //         .GetFolder(folderId, filterBySigningStatus, testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsTrue(folders.Documents.All(d => d.Status == DocumentStatus.Pending));
        //     Assert.AreEqual(folders.TotalDocuments, folders.Documents.Count);
        //     Assert.IsTrue(folder.SystemFolder);
        //     Assert.AreEqual(folderId, folder.Id);
        //     Assert.AreEqual(folders.Id, folder.ParentId);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="FolderExamples.CreateFolder"/>
        // /// </summary>
        // [TestMethod]
        // public async Task CreateFolderTest()
        // {
        //     // Get Root folder and Documents folder
        //     var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
        //     var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
        //
        //     var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;
        //     // Note: You should use different folder name for each example run
        //     var myFolderName = $"CreateFolderExample_{timestamp}";
        //
        //     // Creating new folder
        //     var createNewFolder = await FolderExamples
        //         .CreateFolder(myFolderName, documentsFolder?.Id, testContext)
        //         .ConfigureAwait(false);
        //
        //     Assert.IsNotNull(createNewFolder.Id);
        //
        //     // Check if new folder exists
        //     var checkNewFolderExists = await testContext.Folders
        //         .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
        //         .ConfigureAwait(false);
        //
        //     var myFolder = checkNewFolderExists.Folders.FirstOrDefault(f => f.Name == myFolderName);
        //     Assert.AreEqual(myFolderName, myFolder?.Name);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="FolderExamples.RenameFolder"/>
        // /// </summary>
        // [TestMethod]
        // public async Task RenameFolderTest()
        // {
        //     // Creates folder inside Documents folder for test
        //     var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
        //     var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
        //     var folderForRename = await testContext.Folders
        //         .CreateFolderAsync("noname", documentsFolder?.Id)
        //         .ConfigureAwait(false);
        //
        //     // Rename previously created folder
        //     var renameFolder = await FolderExamples
        //         .RenameFolder("ItsRenamedFolder", folderForRename.Id, testContext)
        //         .ConfigureAwait(false);
        //
        //     var renamed = await testContext.Folders.GetFolderAsync(renameFolder.Id).ConfigureAwait(false);
        //
        //     // Check if folder renamed
        //     Assert.AreEqual("ItsRenamedFolder", renamed.Name);
        //     Assert.AreEqual(folderForRename.Id, renamed.Id);
        //
        //     // Finally - delete test folder
        //     await testContext.Folders.DeleteFolderAsync(renamed.Id).ConfigureAwait(false);
        // }
        //
        // /// <summary>
        // /// Run test for example: <see cref="FolderExamples.DeleteFolder"/>
        // /// </summary>
        // [TestMethod]
        // public async Task DeleteFolderTest()
        // {
        //     // Create some folder for test inside the Documents folder
        //     var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
        //     var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
        //     var folderToDelete = await testContext.Folders
        //         .CreateFolderAsync("DeleteMe", documentsFolder?.Id)
        //         .ConfigureAwait(false);
        //
        //     // Check if test folder exists
        //     var createdFolder = await testContext.Folders
        //         .GetFolderAsync(folderToDelete.Id)
        //         .ConfigureAwait(false);
        //     Assert.AreEqual(folderToDelete.Id, createdFolder.Id);
        //
        //     // Delete folder
        //     await FolderExamples.DeleteFolder(folderToDelete.Id, testContext).ConfigureAwait(false);
        //
        //     // Check if test folder has been deleted
        //     var folders = await testContext.Folders
        //         .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
        //         .ConfigureAwait(false);
        //
        //     Assert.IsFalse(folders.Folders.Any(f => f.Name == "DeleteMe"));
        // }
        //
        // #endregion
        //
        // #region WebHooks Examples
        //
        // /// <summary>
        // /// Allows to subscribe an external service(callback_url) to a specific event of user or document.
        // /// As soon as a certain selected event from the List of event types occurs, SignNow sends a notification about it.
        // /// </summary>
        // /// <see cref="https://docs.signnow.com/docs/signnow/branches/v1.2/reference/operations/create-a-api-v-2-event"/>
        // [TestMethod]
        // public async Task CreateEventSubscription()
        // {
        //     // Upload document with fields
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //     var document = await testContext.Documents
        //         .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionCreate.pdf")
        //         .ConfigureAwait(false);
        //
        //     // Using signNowContext lets create event subscription
        //     var myCallbackUrl = new Uri("https://signnow.com/callbackHandler");
        //     await testContext.Events
        //         .CreateEventSubscriptionAsync(
        //             new CreateEventSubscription(EventType.DocumentComplete, document.Id, myCallbackUrl))
        //         .ConfigureAwait(false);
        //
        //     // Check for successful created event subscription
        //     // Gets information about all subscriptions to events made with a specific application
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/list-api-v-2-events"/>
        //     var eventSubscriptionList = await testContext.Events
        //         .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = 1, PerPage = 1})
        //         .ConfigureAwait(false);
        //
        //     // Determining events total to get the latest event from whole events list
        //     var latestPage = eventSubscriptionList.Meta.Pagination.Total;
        //
        //     // Getting event details from Events list
        //     var myLatestCreatedEvent = await testContext.Events
        //         .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = latestPage, PerPage = 1})
        //         .ConfigureAwait(false);
        //
        //     var myLatestEvent = myLatestCreatedEvent.Data.First();
        //     Assert.AreEqual(EventType.DocumentComplete, myLatestEvent.Event);
        //     Assert.AreEqual(myCallbackUrl, myLatestEvent.JsonAttributes.CallbackUrl);
        //
        //     // Changing an existing event subscription
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/update-a-api-v-2-event"/>
        //     var eventForUpdate = myLatestCreatedEvent.Data.First();
        //     eventForUpdate.JsonAttributes.CallbackUrl = new Uri("https://signnow.com/myNewCallbackHandler");
        //
        //     var changedEvent = await testContext.Events
        //         .UpdateEventSubscriptionAsync(new UpdateEventSubscription(eventForUpdate))
        //         .ConfigureAwait(false);
        //
        //     var updatedEvent = await testContext.Events
        //         .GetEventSubscriptionInfoAsync(changedEvent.Id)
        //         .ConfigureAwait(false);
        //
        //     Assert.AreEqual(myLatestEvent.Id, updatedEvent.Id);
        //     Assert.AreEqual("https://signnow.com/myNewCallbackHandler", updatedEvent.JsonAttributes.CallbackUrl.AbsoluteUri);
        //
        //     // Unsubscribes an external service (callback_url) from specific events of user or document
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/delete-a-api-v-2-event"/>
        //     await testContext.Events
        //         .DeleteEventSubscriptionAsync(myLatestEvent.Id)
        //         .ConfigureAwait(false);
        // }
        //
        // #endregion
        //
        // #region DocumentGroup Examples
        //
        // [TestMethod]
        // public async Task CreateDocumentGroup()
        // {
        //     // Upload test documents
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //
        //     var documents = new List<SignNowDocument>();
        //
        //     for (int i = 0; i < 2; i++)
        //     {
        //         var upload = await testContext.Documents
        //             .UploadDocumentAsync(fileStream, $"ForDocumentGroupFile-{i}.pdf");
        //         var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
        //         documents.Add(doc);
        //     }
        //
        //     var documentGroup = await testContext.DocumentGroup.CreateDocumentGroupAsync(
        //         "CreateDocumentGroupTest", documents).ConfigureAwait(false);
        //
        //     Assert.IsTrue(documentGroup.Id.Length == 40);
        // }
        //
        // #endregion
    }
}
