using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class EmbeddedSigningRequest : JsonHttpContent
    {
        /// <summary>
        /// Collections of <see cref="EmbeddedInvite"/> request options.
        /// </summary>
        [JsonProperty("invites")]
        public List<EmbeddedInvite> Invites { get; set; } = new List<EmbeddedInvite>();

        public EmbeddedSigningRequest() {}

        public EmbeddedSigningRequest(EmbeddedSigningInvite invite)
        {
            Invites = invite.EmbeddedSignInvites;
        }
    }
}
