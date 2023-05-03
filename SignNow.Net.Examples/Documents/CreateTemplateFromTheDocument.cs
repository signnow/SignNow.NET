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
        /// <param name="documentId">Identity of the document which is the source of a template</param>
        /// <param name="templateName">The new template name</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="CreateTemplateFromDocumentResponse"/>Returns a new template ID</returns>
        public static async Task<CreateTemplateFromDocumentResponse> CreateTemplateFromTheDocument(string documentId, string templateName, SignNowContext signNowContext)
        {
            return await signNowContext.Documents
                .CreateTemplateFromDocumentAsync(documentId, templateName)
                .ConfigureAwait(false);
        }
    }
}
