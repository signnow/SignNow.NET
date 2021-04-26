using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Creates a template by flattening an existing document.
        /// </summary>
        /// <param name="request">Request model</param>
        /// <param name="token">Access token</param>
        /// <returns><see cref="DocumentStatus"/></returns>
        public static async Task<CreateTemplateFromDocumentResponse> CreateTemplateFromTheDocument(string documentId, string templateName, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            var createTemplateResult = await signNowContext.Documents
                .CreateTemplateFromDocumentAsync(documentId, templateName).ConfigureAwait(false);

            return createTemplateResult;
        }
    }
}
