using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="CreateDocumentGroupTemplateResponse"/>
    /// </summary>
    public class CreateDocumentGroupTemplateResponseFaker : Faker<CreateDocumentGroupTemplateResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="CreateDocumentGroupTemplateResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "b12e4a885b513a6d9c4c2e7c2b7fa06a013a7412",
        ///   "status": "scheduled"
        /// }
        /// </code>
        /// </example>
        public CreateDocumentGroupTemplateResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Status = f.PickRandom("scheduled", "success", "processing");
            });
        }
    }
}
