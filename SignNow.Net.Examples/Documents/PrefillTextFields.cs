using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.EditFields;

namespace SignNow.Net.Examples.Documents
{
    public static partial class DocumentExamples
    {
        /// <summary>
        /// Adds values to fields that the Signers can later edit when they receive the document for signature.
        /// Works only with Text field types.
        /// </summary>
        /// <param name="documentId">Identity of the document to prefill values for.</param>
        /// <param name="fields">Collection of the <see cref="TextField">fields</see></param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task PrefillTextFields(string documentId, IEnumerable<TextField> fields, Token token)
        {
            // using token from the Authorization step
            var signNowContext = new SignNowContext(token);

            await signNowContext.Documents
                .PrefillTextFieldsAsync(documentId, fields)
                .ConfigureAwait(false);
        }
    }
}
