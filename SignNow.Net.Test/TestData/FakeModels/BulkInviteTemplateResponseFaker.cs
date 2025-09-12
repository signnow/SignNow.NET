using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="SuccessStatusResponse"/>
    /// </summary>
    public class BulkInviteTemplateResponseFaker : Faker<SuccessStatusResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="SuccessStatusResponse"/> fake object.
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