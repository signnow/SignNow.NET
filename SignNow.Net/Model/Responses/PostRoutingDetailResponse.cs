using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for posting routing detail information
    /// </summary>
    public class PostRoutingDetailResponse
    {
        /// <summary>
        /// Array with routing details
        /// </summary>
        [JsonProperty("routing_details")]
        public IReadOnlyList<PostRoutingDetail> RoutingDetails { get; set; }

        /// <summary>
        /// Array of cc's emails
        /// </summary>
        [JsonProperty("cc")]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Array of cc's steps
        /// </summary>
        [JsonProperty("cc_step")]
        public IReadOnlyList<PostCcStep> CcStep { get; set; }

        /// <summary>
        /// Invite link instruction
        /// </summary>
        [JsonProperty("invite_link_instructions")]
        public string InviteLinkInstructions { get; set; }
    }

    /// <summary>
    /// Post routing detail information
    /// </summary>
    public class PostRoutingDetail
    {
        /// <summary>
        /// Default email for routing detail
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Signer role (actor) name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signer role (actor) unique_id
        /// </summary>
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Signer order from actor table
        /// </summary>
        [JsonProperty("signer_order")]
        public int SignerOrder { get; set; }
    }

    /// <summary>
    /// Post CC step information
    /// </summary>
    public class PostCcStep
    {
        /// <summary>
        /// Email of cc step
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Step number
        /// </summary>
        [JsonProperty("step")]
        public int Step { get; set; }

        /// <summary>
        /// Name of cc step
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}