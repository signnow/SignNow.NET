using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.TestData.FakeModels
{
    /// <summary>
    /// Faker for <see cref="UpdateUserInitialsResponse"/>
    /// </summary>
    public sealed class UpdateUserInitialsResponseFaker : Faker<UpdateUserInitialsResponse>
    {
        public UpdateUserInitialsResponseFaker()
        {
            RuleFor(o => o.Id, f => f.Random.Guid().ToString());
            RuleFor(o => o.Width, f => f.Random.Int(50, 500).ToString());
            RuleFor(o => o.Height, f => f.Random.Int(50, 500).ToString());
            RuleFor(o => o.Created, f => f.Date.Past().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}
