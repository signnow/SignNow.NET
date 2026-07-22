using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// The signer to reassign an invite step to.
    /// </summary>
    public class ReassignSignerInfo
    {
        /// <summary>
        /// Email address of the new signer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Name of the role the new signer will fulfill.
        /// </summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }

    /// <summary>
    /// Request to reassign a signer in a specific invite step of a document group workflow.
    /// </summary>
    public class ReassignSignerRequest : JsonHttpContent
    {
        /// <summary>
        /// Details of the signer taking over this invite step.
        /// </summary>
        [JsonProperty("new_signer")]
        public ReassignSignerInfo NewSigner { get; set; }
    }
}
