using System;
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
            // Generate a 40-character string matching the pattern ^[a-zA-Z0-9_]{40,40}$
            RuleFor(o => o.Id, f => f.Random.String2(40, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_"));
            
            // Generate typical initial dimensions - Width and Height are now int
            RuleFor(o => o.Width, f => f.Random.Int(50, 200));
            RuleFor(o => o.Height, f => f.Random.Int(20, 100));
            
            // Generate a past DateTime - Created is now DateTime
            RuleFor(o => o.Created, f => f.Date.Past(1, DateTime.UtcNow));
        }
    }
}
