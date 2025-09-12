using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="SuccessStatusResponse"/>
    /// </summary>
    public class SuccessStatusResponseFaker : Faker<SuccessStatusResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="SuccessStatusResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "status": "success"
        /// }
        /// </code>
        /// </example>
        public SuccessStatusResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Status = "success";
            });
        }
    }
}
