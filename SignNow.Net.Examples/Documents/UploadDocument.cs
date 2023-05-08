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
        /// <param name="signNowContext">signNow container with services.</param>
        public static async Task<SignNowDocument> UploadDocumentWithFieldExtract(string pdfFilePath, SignNowContext signNowContext)
        {
            await using var fileStream = File.OpenRead(pdfFilePath);
            // Upload the document with field extract
            var uploadResponse = signNowContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentSampleWithSignatureTextTag.pdf").Result;

            var documentId = uploadResponse.Id;

            return await signNowContext.Documents.GetDocumentAsync(documentId);
        }

        /// <summary>
        /// Uploads a PDF document to signNow and returns SignNowDocument object.
        /// </summary>
        /// <param name="pdfFilePath">Full qualified path to your PDF file.</param>
        /// <param name="signNowContext">signNow container with services.</param>
        public static async Task<SignNowDocument> UploadDocument(string pdfFilePath, SignNowContext signNowContext)
        {
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
