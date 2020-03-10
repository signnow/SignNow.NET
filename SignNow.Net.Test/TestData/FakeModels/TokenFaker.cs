using System;
using Bogus;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="Token"/>
    /// </summary>
    public class TokenFaker : Faker<Token>
    {
        /// <summary>
        /// Creates new instance of <see cref="Token"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "expires_in": 2592000,
        ///   "access_token": "6ee36f633fb6efbfebac4a2a81c273a7186b7f010356b6a899a258bbb3a524bf",
        ///   "refresh_token": "54559f7eb8c05ce2639e8718dbf45aec760704f91b6ccb6f58efad6d2c8df390",
        ///   "scope": "*",
        ///   "token_type": "Bearer",
        ///   "last_login": 1
        /// }
        /// </code>
        /// </example>
        public TokenFaker()
        {
            Rules((f, o) =>
                {
                    o.ExpiresIn    = (int)UnixTimeStampConverter.ToUnixTimestamp(DateTime.Now.AddDays(f.Random.Int(1, 30)));
                    o.AccessToken  = f.Random.Hash(64);
                    o.RefreshToken = f.Random.Hash(64);
                    o.Scope        = f.PickRandom("user", "*");
                    o.TokenType    = TokenType.Bearer;
                    o.LastLogin    = 1;
                }
            );
        }
    }
}
