using System.IO;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Uploads a PDF with fillable field (Signature field)
        /// </summary>
        /// <param name="pdfFilePath">Full qualified path to your PDF with field tags.</param>
        /// <param name="token">Access token</param>
        public static async Task<SignNowDocument> UploadDocumentWithFieldExtract(string pdfFilePath, Token token)
        {
            var signNowContext = new SignNowContext(token);

            await using var fileStream = File.OpenRead(pdfFilePath);
            // Upload the document with field extract
            var uploadResponse = signNowContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentSampleWithSignatureTextTag.pdf").Result;

            var documentId = uploadResponse.Id;

            return await signNowContext.Documents.GetDocumentAsync(documentId);
        }

        /// <summary>
        /// Uploads a PDF document to SignNow and returns SignNowDocument object.
        /// </summary>
        /// <param name="pdfFilePath">Full qualified path to your PDF file.</param>
        /// <param name="token">Access token</param>
        public static async Task<SignNowDocument> UploadDocument(string pdfFilePath, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);
            var pdfFileName = "document-example.pdf";

            await using var fileStream = File.OpenRead(pdfFilePath);

            // Upload the document
            var uploadResponse = signNowContext.Documents
                .UploadDocumentAsync(fileStream, pdfFileName).Result;

            // Gets document ID from successful response
            var documentId = uploadResponse.Id;

            return await signNowContext.Documents.GetDocumentAsync(documentId);
        }
    }
}
