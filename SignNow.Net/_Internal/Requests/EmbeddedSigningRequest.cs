using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Requests
{
    internal class EmbeddedSigningInvite
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role_id")]
        public  string RoleId { get; set; }

        [JsonProperty("order")]
        public int SigningOrder { get; set; }

        [JsonProperty("auth_method")]
        public string AuthMethod { get; set; } = "none";
    }

    internal class EmbeddedSigningRequest : IContent
    {
        [JsonProperty("invites")]
        public List<EmbeddedSigningInvite> Invites { get; set; } = new List<EmbeddedSigningInvite>();

        public EmbeddedSigningRequest() {}

        public EmbeddedSigningRequest(SignNowDocument document)
        {
            foreach (var role in document.Roles)
            {
                Invites.Add(
                    new EmbeddedSigningInvite
                    {
                        Email = document.FieldInvites.First(i => i.RoleId == role.Id).SignerEmail,
                        RoleId = role.Id,
                        SigningOrder = role.SigningOrder
                    });
            }
        }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this),
                Encoding.UTF8, "application/json");
        }
    }
}
