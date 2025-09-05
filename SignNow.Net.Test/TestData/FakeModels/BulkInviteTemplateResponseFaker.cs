using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="BulkInviteTemplateResponse"/>
    /// </summary>
    public class BulkInviteTemplateResponseFaker : Faker<BulkInviteTemplateResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="BulkInviteTemplateResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "status": "job queued"
        /// }
        /// </code>
        /// </example>
        public BulkInviteTemplateResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Status = "job queued";
            });
        }
    }
}