using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="VerifyEmailResponse"/>
    /// </summary>
    public class VerifyEmailResponseFaker : Faker<VerifyEmailResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="VerifyEmailResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "email": "verified_email@emaildomain.com"
        /// }
        /// </code>
        /// </example>
        public VerifyEmailResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
            });
        }
    }
}
