using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Examples.Authentication;
using SignNow.Net.Examples.Documents;
using SignNow.Net.Examples.Folders;
using SignNow.Net.Examples.Invites;
using SignNow.Net.Examples.Users;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Test.Context;

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
    public class ExamplesRunner
    {
        private DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Base path to the `TestExamples` directory.
        /// Path should use Unix-like directory separator char. It requires for cross platform path compatibility.
        /// </summary>
        private static readonly string BaseTestExamplesPath = "../../../TestExamples/"
            .Replace('/', Path.DirectorySeparatorChar);

        private static readonly string PdfWithSignatureField = Path.Combine(BaseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");
        private static readonly string PdfWithoutFields = Path.Combine(BaseTestExamplesPath, "SignAndDate.pdf");

        /// <summary>
        /// Contains application clientId, clientSecret and user credentials
        /// </summary>
        private static CredentialModel credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();

        /// <summary>Token for ExampleRunner</summary>
        private readonly Token token;

        /// <summary>
        /// SignNow service container used for ExampleRunner
        /// </summary>
        private readonly SignNowContext testContext;

        /// <summary>
        /// Document Id which should be deleted after each test
        /// </summary>
        private string disposableDocumentId;

        /// <summary>
        /// SignNow API base Url (sandbox)
        /// </summary>
        private static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");

        public ExamplesRunner()
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

            // Token for test runner
            token = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, credentials).Result;
            testContext = new SignNowContext(ApiBaseUrl, token);
        }

        /// <summary>
        /// Delete test document after each test.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            if (string.IsNullOrEmpty(disposableDocumentId))
            {
                return;
            }

            var documentTask = testContext.Documents
                .DeleteDocumentAsync(disposableDocumentId);

            Task.WaitAll(documentTask);

            Assert.IsFalse(documentTask.IsFaulted);

            disposableDocumentId = string.Empty;
        }

        #region Authentication Examples

        /// <summary>
        /// Run test for example: <see cref="AuthenticationExamples.RequestAccessToken"/>
        /// </summary>
        [TestMethod]
        public async Task RequestAccessTokenTest()
        {
            var requestAccessToken = await AuthenticationExamples
                .RequestAccessToken(ApiBaseUrl, credentials)
                .ConfigureAwait(false);

            Assert.IsNotNull(requestAccessToken);
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.RefreshToken));
        }

        #endregion

        #region Documents Examples

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.UploadDocumentWithFieldExtract"/>
        /// </summary>
        [TestMethod]
        public async Task UploadDocumentWithFieldExtractTest()
        {
            var documentWithFields = await DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).ConfigureAwait(false);

            disposableDocumentId = documentWithFields?.Id;

            using var documentFields = documentWithFields?.Fields.GetEnumerator();
            documentFields?.MoveNext();

            Assert.AreEqual(FieldType.Text, documentFields?.Current?.Type);
            Assert.IsTrue(documentWithFields?.Fields.Count > 0);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.DownloadSignedDocument"/>
        /// </summary>
        [TestMethod]
        public async Task DownloadSignedDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = await testContext.Documents
                .UploadDocumentAsync(fileStream, "SignedDocumentTest.pdf").ConfigureAwait(false);

            disposableDocumentId = document.Id;

            var documentSigned = await DocumentExamples
                .DownloadSignedDocument(document.Id, token).ConfigureAwait(false);

            Assert.AreEqual("SignedDocumentTest.pdf", documentSigned.Filename);
            Assert.IsInstanceOfType(documentSigned.Document, typeof(Stream));
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateSigningLinkToTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateSigningLinkToTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateSigningLinkToTheDocument.pdf")
                .ConfigureAwait(false);

            disposableDocumentId = document?.Id;

            var signingLink = await DocumentExamples
                .CreateSigningLinkToTheDocument(document?.Id, token).ConfigureAwait(false);

            Assert.IsNotNull(signingLink.Url);
            Assert.IsNotNull(signingLink.AnonymousUrl);
            Assert.IsInstanceOfType(signingLink.Url, typeof(Uri));
            Assert.AreEqual("https", signingLink.Url.Scheme);
            Assert.IsFalse(string.IsNullOrEmpty(signingLink.Url.OriginalString));
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CheckTheStatusOfTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CheckTheStatusOfTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CheckTheStatusOfTheDocument.pdf")
                .ConfigureAwait(false);

            disposableDocumentId = document?.Id;

            var documentStatus = await DocumentExamples.CheckTheStatusOfTheDocument(document?.Id, token).ConfigureAwait(false);

            Assert.AreEqual(DocumentStatus.NoInvite, documentStatus);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.MergeTwoDocuments"/>
        /// </summary>
        [TestMethod]
        public async Task MergeTwoDocumentsTest()
        {
            await using var file1Stream = File.OpenRead(PdfWithSignatureField);
            await using var file2Stream = File.OpenRead(PdfWithoutFields);

            var doc1 = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file1Stream, "MergeTwoDocumentsTest1.pdf")
                .ConfigureAwait(false);
            var doc2 = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file2Stream, "MergeTwoDocumentsTest2.pdf")
                .ConfigureAwait(false);

            var document1 = await testContext.Documents.GetDocumentAsync(doc1.Id).ConfigureAwait(false);
            var document2 = await testContext.Documents.GetDocumentAsync(doc2.Id).ConfigureAwait(false);

            var documents = new List<SignNowDocument> { document1, document2 };
            var finalDocument = await DocumentExamples
                .MergeTwoDocuments("MergeTwoDocumentsTestResult.pdf", documents, token)
                .ConfigureAwait(false);

            await testContext.Documents.DeleteDocumentAsync(doc1.Id);
            await testContext.Documents.DeleteDocumentAsync(doc2.Id);

            Assert.AreEqual("MergeTwoDocumentsTestResult.pdf", finalDocument.Filename);
            Assert.IsInstanceOfType(finalDocument.Document, typeof(Stream) );
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.GetTheDocumentHistory"/>
        /// </summary>
        [TestMethod]
        public async Task GetTheDocumentHistoryTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "GetTheDocumentHistory.pdf")
                .ConfigureAwait(false);

            disposableDocumentId = document?.Id;

            var documentHistory = await DocumentExamples
                .GetTheDocumentHistory(disposableDocumentId, token)
                .ConfigureAwait(false);

            Assert.IsTrue(documentHistory.All(item => item.DocumentId == disposableDocumentId));
            Assert.IsTrue(documentHistory.Any(item => item.Origin == "original"));
            Assert.IsTrue(documentHistory.All(item => item.Email == credentials.Login));
        }

        /// <summary>
        /// Run the test for example: <see cref="DocumentExamples.CreateOneTimeLinkToDownloadTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateOneTimeLinkToDownloadTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateOneTimeLinkToDownloadTheDocumentTest.pdf")
                .ConfigureAwait(false);

            disposableDocumentId = document?.Id;

            var oneTimeLink = await DocumentExamples
                .CreateOneTimeLinkToDownloadTheDocument(disposableDocumentId, token)
                .ConfigureAwait(false);

            StringAssert.Contains(oneTimeLink.Url.Host, "signnow.com");
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.MoveTheDocumentToFolder"/>
        /// </summary>
        [TestMethod]
        public async Task MoveDocumentTest()
        {
            // Upload test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var testDocument = await testContext.Documents
                .UploadDocumentAsync(fileStream, "MoveDocumentTest.pdf");

            // Create new Folder where you'd like to keep test document
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
            var folderToMove = await testContext.Folders
                .CreateFolderAsync("FolderToMoveDocument", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Move test document to folder created with previous step
            await DocumentExamples
                .MoveTheDocumentToFolder(testDocument.Id, folderToMove.Id, token)
                .ConfigureAwait(false);

            // Check if test document has been moved
            var folderToMoveUpdated = await testContext.Folders
                .GetFolderAsync(folderToMove.Id)
                .ConfigureAwait(false);
            Assert.AreEqual("FolderToMoveDocument", folderToMoveUpdated.Name);
            Assert.AreEqual(1, folderToMoveUpdated.TotalDocuments);
            Assert.AreEqual(testDocument.Id, folderToMoveUpdated.Documents.FirstOrDefault(d => d.Id == testDocument.Id)?.Id);

            // Finally - delete document and folder
            await testContext.Documents.DeleteDocumentAsync(testDocument.Id).ConfigureAwait(false);
            await testContext.Folders.DeleteFolderAsync(folderToMoveUpdated.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.EditDocumentTextFields"/>
        /// Run test for example: <see cref="DocumentExamples.PrefillTextFields"/>
        /// </summary>
        [TestMethod]
        public async Task PrefillTextFieldsTest()
        {
            // Upload test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "PrefillDocumentTest.pdf")
                .ConfigureAwait(false);

            var documentUploaded = await testContext.Documents.GetDocumentAsync(testDocument.Id).ConfigureAwait(false);
            Assert.IsNull(documentUploaded.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);

            // Add simple text field which will be prefilled next.
            var editFields = new List<IFieldEditable>
            {
                new TextField
                {
                    PageNumber = 0,
                    Name = "Text_1",
                    Height = 40,
                    Width = 200,
                    X = 10,
                    Y = 40,
                    Role = "Signer 1"
                }
            };

            var editDocument = await DocumentExamples
                .EditDocumentTextFields(testDocument.Id, editFields, token)
                .ConfigureAwait(false);

            var documentEdited = await testContext.Documents.GetDocumentAsync(editDocument.Id).ConfigureAwait(false);
            Assert.IsNull(documentEdited.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);
            Assert.AreEqual("Text_1", documentEdited.Fields.FirstOrDefault()?.JsonAttributes.Name);

            var fields = new List<TextField>
            {
                new TextField
                {
                    Name = "Text_1",
                    PrefilledText = "Test Prefill"
                }
            };

            await DocumentExamples.PrefillTextFields(testDocument.Id, fields, token).ConfigureAwait(false);

            var documentFinal = await testContext.Documents.GetDocumentAsync(testDocument.Id).ConfigureAwait(false);
            Assert.AreEqual("Test Prefill", documentFinal.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);
        }

        #endregion

        #region Invites Examples

        /// <summary>
        /// Run test for example: <see cref="InviteExamples.CreateFreeformInviteToSignTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateFreeformInviteToSignTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = await testContext.Documents
                .UploadDocumentAsync(fileStream, "CreateFreeformInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            disposableDocumentId = document?.Id;

            var inviteResponse = await InviteExamples
                .CreateFreeformInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", token)
                .ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrEmpty(inviteResponse.Id));

            var documentWithInvite = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            var createdInvite = documentWithInvite.InvitesStatus.FirstOrDefault();

            Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
            Assert.AreEqual(inviteResponse.Id, createdInvite?.Id);
            Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);
        }

        /// <summary>
        /// Run test for example: <see cref="InviteExamples.CreateRoleBasedInviteToSignTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateRoleBasedInviteToSignTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateRoleBasedInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            disposableDocumentId = document?.Id;

            var inviteResponse = await InviteExamples
                .CreateRoleBasedInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", token)
                .ConfigureAwait(false);

            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            var documentWithInvite = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            var createdInvite = documentWithInvite.FieldInvites.FirstOrDefault();

            Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
            Assert.AreEqual("Signer 1", createdInvite?.RoleName, "Signer role mismatch.");
            Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);
        }

        /// <summary>
        /// Run test for example:
        /// <see cref="InviteExamples.CreateEmbeddedSigningInviteToSignTheDocument"/>
        /// <see cref="InviteExamples.GenerateLinkForEmbeddedInvite"/>
        /// </summary>
        [TestMethod]
        public async Task CreateEmbeddedSigningInviteToSignTheDocumentTest()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateEmbeddedSigningInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);

            disposableDocumentId = document?.Id;

            // Create Embedded Signing Invite
            var embeddedInviteResponse = await InviteExamples
                .CreateEmbeddedSigningInviteToSignTheDocument(signNowDoc, "testemail@signnow.com", token)
                .ConfigureAwait(false);

            Assert.AreEqual(1, embeddedInviteResponse.InviteData.Count);
            Assert.AreEqual(1, embeddedInviteResponse.InviteData[0].Order);
            Assert.AreEqual("testemail@signnow.com", embeddedInviteResponse.InviteData[0].Email);
            Assert.AreEqual("Pending", embeddedInviteResponse.InviteData[0].Status.ToString());


            var documentWithEmbed = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            Assert.IsTrue(documentWithEmbed.FieldInvites.First().IsEmbedded);

            // Generate link for Embedded Signing Invite
            var embeddedLink = await InviteExamples
                .GenerateLinkForEmbeddedInvite(documentWithEmbed, 30, token).ConfigureAwait(false);

            Assert.IsInstanceOfType(embeddedLink.Link, typeof(Uri));
            Console.WriteLine($@"Embedded link: {embeddedLink.Link.AbsoluteUri}");

            // Cancel embedded invite
            await InviteExamples.CancelEmbeddedInvite(documentWithEmbed, token).ConfigureAwait(false);
        }

        #endregion

        #region User Examples

        /// <summary>
        /// Run test for <see cref="UserExamples.CreateSignNowUser"/> and <see cref="UserExamples.SendVerificationEmailToUser"/>
        /// </summary>
        [TestMethod]
        public async Task CreateSignNowUserTest()
        {
            var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;

            var createUserResponse = await UserExamples.CreateSignNowUser(
                "John",
                $"Sample{timestamp}",
                $"signnow.tutorial+sample_test{timestamp}@gmail.com",
                "secretPassword",
                token
            ).ConfigureAwait(false);

            Assert.AreEqual($"signnow.tutorial+sample_test{timestamp}@gmail.com", createUserResponse.Email);
            Assert.IsFalse(createUserResponse.Verified);

            // Finally - send verification email to User
            await UserExamples.SendVerificationEmailToUser(createUserResponse.Email, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Run test for example: <see cref="UserExamples.GetUserModifiedDocuments"/>
        /// </summary>
        [TestMethod]
        public async Task GetUserModifiedDocumentsTest()
        {
            var perPage = 25;
            var SignNowDocumentsAsync = await UserExamples
                .GetUserModifiedDocuments(perPage, token)
                .ConfigureAwait(false);

            var modifiedDocuments = SignNowDocumentsAsync.ToList();
            foreach (var document in modifiedDocuments)
            {
                Assert.AreEqual(credentials.Login, document.Owner);
            }

            Assert.IsNotNull(modifiedDocuments.Count);
            Console.WriteLine($@"Total modified documents: {modifiedDocuments.Count}");
        }

        /// <summary>
        /// Run test for example: <see cref="UserExamples.GetUserDocuments"/>
        /// </summary>
        [TestMethod]
        public async Task GetUserDocumentsTest()
        {
            var perPage = 25;
            var SignNowDocumentsAsync = await UserExamples
                .GetUserDocuments(perPage, token)
                .ConfigureAwait(false);

            var userDocuments = SignNowDocumentsAsync.ToList();
            foreach (var document in userDocuments)
            {
                Assert.AreEqual(credentials.Login, document.Owner);
            }

            Assert.IsNotNull(userDocuments.Count);
            Console.WriteLine($@"Total modified documents: {userDocuments.Count}");
        }

        #endregion

        #region Templates Examples

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateTemplateFromTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateTemplateFromDocumentTest()
        {
            var document = await DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).ConfigureAwait(false);
            disposableDocumentId = document?.Id;

            const string templateName = "Template Name";
            var result = await DocumentExamples.CreateTemplateFromTheDocument(document?.Id, templateName, token).ConfigureAwait(false);
            var template = await testContext.Documents.GetDocumentAsync(result.Id).ConfigureAwait(false);

            Assert.IsFalse(document.IsTemplate);
            Assert.IsNotNull(template?.Id);
            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsTemplate);

            await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateDocumentFromTheTemplate"/>
        /// </summary>
        [TestMethod]
        public async Task CreateDocumentFromTemplateTest()
        {
            var testDocument = await DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token)
                .ConfigureAwait(false);
            disposableDocumentId = testDocument.Id;

            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "TemplateName")
                .ConfigureAwait(false);
            var documentName = "Document Name";
            var result = await DocumentExamples
                .CreateDocumentFromTheTemplate(template.Id, documentName, token)
                .ConfigureAwait(false);
            var document = await testContext.Documents
                .GetDocumentAsync(result.Id)
                .ConfigureAwait(false);

            Assert.IsNotNull(document?.Id);
            Assert.IsFalse(document.IsTemplate);
            Assert.AreEqual(documentName, document.Name);

            await testContext.Documents.DeleteDocumentAsync(document.Id).ConfigureAwait(false);
            await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
        }

        #endregion

        #region Folder Examples

        /// <summary>
        /// Run test for example: <see cref="FolderExamples.GetAllFolders"/>
        /// </summary>
        [TestMethod]
        public async Task GetAllFoldersTest()
        {
            var folders = await FolderExamples
                .GetAllFolders(token)
                .ConfigureAwait(false);

            Assert.IsInstanceOfType(folders, typeof(SignNowFolders));
            Assert.AreEqual("Root", folders.Name);
            Assert.IsTrue(folders.SystemFolder);

            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Documents"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Archive"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Templates"));
        }

        /// <summary>
        /// Run test for example: <see cref="FolderExamples.GetFolder"/>
        /// </summary>
        [TestMethod]
        public async Task GetFolderTest()
        {
            var folders = await FolderExamples.GetAllFolders(token).ConfigureAwait(false);
            var folderId = folders.Folders.FirstOrDefault(f => f.Name == "Documents")?.Id;

            var filterBySigningStatus = new GetFolderOptions
            {
                Filters = new FolderFilters(SigningStatus.Pending)
            };

            var folder = await FolderExamples
                .GetFolder(folderId, filterBySigningStatus, token)
                .ConfigureAwait(false);

            Assert.IsTrue(folders.Documents.All(d => d.Status == DocumentStatus.Pending));
            Assert.AreEqual(folders.TotalDocuments, folders.Documents.Count);
            Assert.IsTrue(folder.SystemFolder);
            Assert.AreEqual(folderId, folder.Id);
            Assert.AreEqual(folders.Id, folder.ParentId);
        }

        /// <summary>
        /// Run test for example: <see cref="FolderExamples.CreateFolder"/>
        /// </summary>
        [TestMethod]
        public async Task CreateFolderTest()
        {
            // Get Root folder and Documents folder
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");

            var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;
            // Note: You should use different folder name for each example run
            var myFolderName = $"CreateFolderExample_{timestamp}";

            // Creating new folder
            var createNewFolder = await FolderExamples
                .CreateFolder(myFolderName, documentsFolder?.Id, token)
                .ConfigureAwait(false);

            Assert.IsNotNull(createNewFolder.Id);

            // Check if new folder exists
            var checkNewFolderExists = await testContext.Folders
                .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
                .ConfigureAwait(false);

            var myFolder = checkNewFolderExists.Folders.FirstOrDefault(f => f.Name == myFolderName);
            Assert.AreEqual(myFolderName, myFolder?.Name);
        }

        /// <summary>
        /// Run test for example: <see cref="FolderExamples.RenameFolder"/>
        /// </summary>
        [TestMethod]
        public async Task RenameFolderTest()
        {
            // Creates folder inside Documents folder for test
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
            var folderForRename = await testContext.Folders
                .CreateFolderAsync("noname", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Rename previously created folder
            var renameFolder = await FolderExamples
                .RenameFolder("ItsRenamedFolder", folderForRename.Id, token)
                .ConfigureAwait(false);

            var renamed = await testContext.Folders.GetFolderAsync(renameFolder.Id).ConfigureAwait(false);

            // Check if folder renamed
            Assert.AreEqual("ItsRenamedFolder", renamed.Name);
            Assert.AreEqual(folderForRename.Id, renamed.Id);

            // Finally - delete test folder
            await testContext.Folders.DeleteFolderAsync(renamed.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Run test for example: <see cref="FolderExamples.DeleteFolder"/>
        /// </summary>
        [TestMethod]
        public async Task DeleteFolderTest()
        {
            // Create some folder for test inside the Documents folder
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
            var folderToDelete = await testContext.Folders
                .CreateFolderAsync("DeleteMe", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Check if test folder exists
            var createdFolder = await testContext.Folders
                .GetFolderAsync(folderToDelete.Id)
                .ConfigureAwait(false);
            Assert.AreEqual(folderToDelete.Id, createdFolder.Id);

            // Delete folder
            await FolderExamples.DeleteFolder(folderToDelete.Id, token).ConfigureAwait(false);

            // Check if test folder has been deleted
            var folders = await testContext.Folders
                .GetFolderAsync(documentsFolder?.Id, new GetFolderOptions {IncludeDocumentsSubfolder = false})
                .ConfigureAwait(false);

            Assert.IsFalse(folders.Folders.Any(f => f.Name == "DeleteMe"));
        }

        #endregion
    }
}
