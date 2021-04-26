using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Creates a template by flattening an existing document.
        /// </summary>
        /// <param name="templateId">Identity of the template.</param>
        /// <param name="documentName">The name of new document</param>
        /// <param name="token">Access token</param>
        /// <returns><see cref="DocumentStatus"/></returns>
        public static async Task<CreateDocumentFromTemplateResponse> CreateDocumentFromTheTemplate(string documentName, string templateId, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            var createDocumentResult = await signNowContext.Documents
                .CreateDocumentFromTemplateAsync(documentName, templateId).ConfigureAwait(false);

            return createDocumentResult;
        }
    }
}
