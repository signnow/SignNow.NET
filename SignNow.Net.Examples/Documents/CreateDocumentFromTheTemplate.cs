using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Creates document from template.
        /// </summary>
        /// <param name="templateId">Identity of the template</param>
        /// <param name="documentName">The name of new document</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns><see cref="CreateDocumentFromTemplateResponse"/>New document ID</returns>
        public static async Task<CreateDocumentFromTemplateResponse> CreateDocumentFromTheTemplate(string templateId, string documentName, SignNowContext signNowContext)
        {
            return await signNowContext.Documents
                .CreateDocumentFromTemplateAsync(templateId, documentName)
                .ConfigureAwait(false);
        }
    }
}
