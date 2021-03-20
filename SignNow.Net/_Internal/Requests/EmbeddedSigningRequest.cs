using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Requests
{
    internal class EmbeddedSigningRequest : IContent
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

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }
}
