using System;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="EmbeddedInvite"/>
    /// </summary>
    public class EmbeddedInviteFaker : Faker<EmbeddedInvite>
    {
        /// <summary>
        /// Faker <see cref="EmbeddedInvite"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "email": "Jesus_Monahan@gmail.com",
        ///   "language": "en",
        ///   "auth_method": "none",
        ///   "first_name": "Jesus",
        ///   "last_name": "Monahan",
        ///   "prefill_signature_name": "Jesus Monahan prefill signature",
        ///   "force_new_signature": "1",
        ///   "redirect_uri": "https://signnow.com",
        ///   "decline_redirect_uri": "https://signnow.com",
        ///   "redirect_target": "self",
        ///   "role_id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "order": 1
        /// }
        /// </code>
        /// </example>
        public EmbeddedInviteFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.Language = f.PickRandom<Lang>();
                o.AuthMethod = f.PickRandom<EmbeddedAuthType>();
                o.Firstname = f.Person.FirstName;
                o.Lastname = f.Person.LastName;
                o.PrefillSignatureName = $"{f.Person.FirstName} {f.Person.LastName} prefill signature";
                o.ForceNewSignature = f.Random.Bool();
                o.RedirectUrl = new Uri(f.Internet.Url());
                o.DeclineRedirectUrl = new Uri(f.Internet.Url());
                o.RedirectTarget = f.PickRandom<RedirectTarget>();
                o.RoleId = f.Random.Hash(40);
                o.SigningOrder = (uint)f.Random.Number(1, 10);
            });
        }
    }
}
