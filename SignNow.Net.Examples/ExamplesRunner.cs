using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Examples.Authentication;
using SignNow.Net.Examples.Documents;
using SignNow.Net.Examples.Invites;
using SignNow.Net.Examples.Users;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
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
        public void RequestAccessTokenTest()
        {
            var requestAccessToken = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, credentials).Result;

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
        public void UploadDocumentWithFieldExtractTest()
        {
            var documentWithFields = DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).Result;

            disposableDocumentId = documentWithFields?.Id;

            using var documentFields = documentWithFields.Fields.GetEnumerator();
            documentFields.MoveNext();

            Assert.AreEqual(FieldType.Text, documentFields.Current.Type);
            Assert.IsTrue(documentWithFields.Fields.Count > 0);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.DownloadSignedDocument"/>
        /// </summary>
        [TestMethod]
        public void DownloadSignedDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = testContext.Documents
                .UploadDocumentAsync(fileStream, "SignedDocumentTest.pdf").Result;

            disposableDocumentId = document.Id;

            var documentSigned = DocumentExamples
                .DownloadSignedDocument(document.Id, token).Result;

            Assert.AreEqual("SignedDocumentTest.pdf", documentSigned.Filename);
            Assert.IsInstanceOfType(documentSigned.Document, typeof(Stream));
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateSigningLinkToTheDocument"/>
        /// </summary>
        [TestMethod]
        public void CreateSigningLinkToTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateSigningLinkToTheDocument.pdf").Result;

            disposableDocumentId = document?.Id;

            var signingLink = DocumentExamples
                .CreateSigningLinkToTheDocument(document.Id, token).Result;

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
        public void CheckTheStatusOfTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CheckTheStatusOfTheDocument.pdf").Result;

            disposableDocumentId = document?.Id;

            var documentStatus = DocumentExamples.CheckTheStatusOfTheDocument(document.Id, token).Result;

            Assert.AreEqual(DocumentStatus.NoInvite, documentStatus);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.MergeTwoDocuments"/>
        /// </summary>
        [TestMethod]
        public void MergeTwoDocumentsTest()
        {
            using var file1Stream = File.OpenRead(PdfWithSignatureField);
            using var file2Stream = File.OpenRead(PdfWithoutFields);

            var doc1 = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file1Stream, "MergeTwoDocumentsTest1.pdf").Result;
            var doc2 = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file2Stream, "MergeTwoDocumentsTest2.pdf").Result;

            var document1 = testContext.Documents.GetDocumentAsync(doc1.Id).Result;
            var document2 = testContext.Documents.GetDocumentAsync(doc2.Id).Result;

            var documents = new List<SignNowDocument> { document1, document2 };
            var finalDocument = DocumentExamples
                .MergeTwoDocuments("MergeTwoDocumentsTestResult.pdf", documents, token).Result;

            testContext.Documents.DeleteDocumentAsync(doc1.Id);
            testContext.Documents.DeleteDocumentAsync(doc2.Id);

            Assert.AreEqual("MergeTwoDocumentsTestResult.pdf", finalDocument.Filename);
            Assert.IsInstanceOfType(finalDocument.Document, typeof(Stream) );
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.GetTheDocumentHistory"/>
        /// </summary>
        [TestMethod]
        public void GetTheDocumentHistoryTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "GetTheDocumentHistory.pdf").Result;

            disposableDocumentId = document?.Id;

            var documentHistory = DocumentExamples
                .GetTheDocumentHistory(disposableDocumentId, token).Result;

            Assert.IsTrue(documentHistory.All(item => item.DocumentId == disposableDocumentId));
            Assert.IsTrue(documentHistory.Any(item => item.Origin == "original"));
            Assert.IsTrue(documentHistory.All(item => item.Email == credentials.Login));
        }

        /// <summary>
        /// Run the test for example: <see cref="DocumentExamples.CreateOneTimeLinkToDownloadTheDocument"/>
        /// </summary>
        [TestMethod]
        public void CreateOneTimeLinkToDownloadTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateOneTimeLinkToDownloadTheDocumentTest.pdf").Result;

            disposableDocumentId = document?.Id;

            var oneTimeLink = DocumentExamples
                .CreateOneTimeLinkToDownloadTheDocument(disposableDocumentId, token).Result;

            StringAssert.Contains(oneTimeLink.Url.Host, "signnow.com");
        }

        #endregion

        #region Invites Examples

        /// <summary>
        /// Run test for example: <see cref="InviteExamples.CreateFreeformInviteToSignTheDocument"/>
        /// </summary>
        [TestMethod]
        public void CreateFreeformInviteToSignTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = testContext.Documents
                .UploadDocumentAsync(fileStream, "CreateFreeformInviteToSignTheDocument.pdf").Result;

            var signNowDoc = testContext.Documents.GetDocumentAsync(document.Id).Result;
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            disposableDocumentId = document?.Id;

            var inviteResponse = InviteExamples
                .CreateFreeformInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", token).Result;

            Assert.IsFalse(string.IsNullOrEmpty(inviteResponse.Id));

            var documentWithInvite = testContext.Documents.GetDocumentAsync(document.Id).Result;
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
        public void CreateRoleBasedInviteToSignTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateRoleBasedInviteToSignTheDocument.pdf").Result;

            var signNowDoc = testContext.Documents.GetDocumentAsync(document.Id).Result;
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            disposableDocumentId = document?.Id;

            var inviteResponse = InviteExamples
                .CreateRoleBasedInviteToSignTheDocument(signNowDoc, "noreply@signnow.com", token).Result;

            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            var documentWithInvite = testContext.Documents.GetDocumentAsync(document.Id).Result;
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
        public void CreateEmbeddedSigningInviteToSignTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateEmbeddedSigningInviteToSignTheDocument.pdf").Result;

            var signNowDoc = testContext.Documents.GetDocumentAsync(document.Id).Result;

            disposableDocumentId = document?.Id;

            // Create Embedded Signing Invite
            var embeddedInviteResponse = InviteExamples
                .CreateEmbeddedSigningInviteToSignTheDocument(signNowDoc, "testemail@signnow.com", token).Result;

            Assert.AreEqual(1, embeddedInviteResponse.InviteData.Count);
            Assert.AreEqual(1, embeddedInviteResponse.InviteData[0].Order);
            Assert.AreEqual("testemail@signnow.com", embeddedInviteResponse.InviteData[0].Email);
            Assert.AreEqual("Pending", embeddedInviteResponse.InviteData[0].Status.ToString());


            var documentWithEmbed = testContext.Documents.GetDocumentAsync(document.Id).Result;
            Assert.IsTrue(documentWithEmbed.FieldInvites.First().IsEmbedded);

            // Generate link for Embedded Signing Invite
            var embeddedLink = InviteExamples
                .GenerateLinkForEmbeddedInvite(documentWithEmbed, 30, token).Result;

            Assert.IsInstanceOfType(embeddedLink.Link, typeof(Uri));
            Console.WriteLine($"Embedded link: {embeddedLink.Link.AbsoluteUri}");

            // Cancel embedded invite
            var cancelled = InviteExamples.CancelEmbeddedInvite(documentWithEmbed, token);
        }

        #endregion

        #region User Examples

        /// <summary>
        /// Run test for <see cref="UserExamples.CreateSignNowUser"/> and <see cref="UserExamples.SendVerificationEmailToUser"/>
        /// </summary>
        [TestMethod]
        public void CreateSignNowUserTest()
        {
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;

            var createUserResponse = UserExamples.CreateSignNowUser(
                "John",
                $"Sample{timestamp}",
                $"signnow.tutorial+sample_test{timestamp}@gmail.com",
                "secretPassword",
                token
            ).Result;

            Assert.AreEqual($"signnow.tutorial+sample_test{timestamp}@gmail.com", createUserResponse.Email);
            Assert.IsFalse(createUserResponse.Verified);

            // Finally - send verification email to User
            UserExamples.SendVerificationEmailToUser(createUserResponse.Email, token).GetAwaiter().GetResult();
        }

        #endregion

        #region Templates Examples

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateTemplateFromTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateTemplateFromDocumentTest()
        {
            var document = DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).Result;
            disposableDocumentId = document?.Id;

            var templateName = "Template Name";
            var result = await DocumentExamples.CreateTemplateFromTheDocument(document?.Id, templateName, token);
            var template = await testContext.Documents.GetDocumentAsync(result.Id);

            Assert.IsFalse(document.IsTemplate);
            Assert.IsNotNull(template?.Id);
            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsTemplate);

            await testContext.Documents.DeleteDocumentAsync(template.Id);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateTemplateFromTheDocument"/>
        /// </summary>
        [TestMethod]
        public async Task CreateDocumentFromTemplateTest()
        {
            var testDocumentId = DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).Result?.Id;
            disposableDocumentId = testDocumentId;
            var templateId = (await testContext.Documents.CreateTemplateFromDocumentAsync(testDocumentId, "TemplateName")).Id;
            var documentName = "Document Name";
            var result = await DocumentExamples.CreateDocumentFromTheTemplate(templateId, documentName, token);
            var document = await testContext.Documents.GetDocumentAsync(result.Id);

            Assert.IsNotNull(document?.Id);
            Assert.AreEqual(documentName, document.Name);

            await testContext.Documents.DeleteDocumentAsync(document.Id);
            await testContext.Documents.DeleteDocumentAsync(templateId);
        }

        #endregion
    }
}
