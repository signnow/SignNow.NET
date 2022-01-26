using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples.Documents
{
    // public class EditDocumentTextFields
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Updates a document by adding/overwriting fields or elements (texts, checks, signatures, hyperlinks, attachments)
        /// </summary>
        /// <param name="documentId">Identity of the document to add/edit fields for.</param>
        /// <param name="fields">Collection of the <see cref="IFieldEditable">fields</see></param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<EditDocumentResponse> EditDocumentTextFields(string documentId, IEnumerable<IFieldEditable> fields, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Documents
                .EditDocumentAsync(documentId, fields)
                .ConfigureAwait(false);
        }
    }
}
