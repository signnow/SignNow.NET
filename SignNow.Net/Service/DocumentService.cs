using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Service
{
    public class DocumentService : AuthorizedWebClientBase, IDocumentService
    {
        public DocumentService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {

        }
        public DocumentService(Uri baseApiUrl, Token token) : base(baseApiUrl, token)
        {

        }
        internal protected DocumentService(Uri baseApiUrl, Token token, ISignNowClient signNowClient) : base(baseApiUrl, token, signNowClient)
        {

        }
        public Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uploads a file and creates a document. This method accepts .doc, .docx, .pdf, and .png file types.
        /// </summary>
        /// <param name="documentContent">Document content stream</param>
        /// <param name="extractFields">Set true to extract simple field tags (default value) from document if any. This setting affects only .doc, .docx and .pdf file types. See <see cref="https://campus.barracuda.com/product/signnow/doc/41113461/rest-endpoints-api"/></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Operation result object containing ID of the new document.</returns>
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, bool extractFields = true, CancellationToken cancellationToken = default)
        {
            var documentRequestRelativeUrl = "/document" + (extractFields ? "/fieldextract" : string.Empty);
            var requestFullUrl = new Uri(ApiBaseUrl, documentRequestRelativeUrl);
            var requestOptions = new RequestOptions();
            //var requestOptions = new RequestOptions
            //{
            //    RequestUrl = requestFullUrl,
            //    HttpMethod = "POST",
            //    Content = documentContent
            //};
            return await SignNowClient.RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
