using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net._Internal.Requests;

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

        ///<inheritdoc/>
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document", documentContent, cancellationToken).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document/fieldextract", documentContent, cancellationToken).ConfigureAwait(false);
        }

        private async Task<UploadDocumentResponse> UploadDocumentAsync (string requestRelativeUrl, Stream documentContent, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, requestRelativeUrl);
            var requestOptions = new PostHttpRequesOptions
            {
                RequestUrl = requestFullUrl,
                Content = new MultipartFormDataHttpContent(documentContent),
                Token = this.Token
            };
            return await SignNowClient.RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
