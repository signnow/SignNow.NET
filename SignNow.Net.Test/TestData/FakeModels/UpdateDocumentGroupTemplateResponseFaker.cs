using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="UpdateDocumentGroupTemplateResponse"/>
    /// </summary>
    public class UpdateDocumentGroupTemplateResponseFaker : Faker<UpdateDocumentGroupTemplateResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateDocumentGroupTemplateResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "status": "success"
        /// }
        /// </code>
        /// </example>
        public UpdateDocumentGroupTemplateResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Status = "success";
            });
        }
    }
}
