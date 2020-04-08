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
    }
}
