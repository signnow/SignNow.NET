using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for creating routing detail information
    /// </summary>
    [JsonObject]
    public class CreateRoutingDetailResponse
    {
        /// <summary>
        /// Array with routing details
        /// </summary>
        [JsonProperty("routing_details")]
        public IReadOnlyList<CreateRoutingDetail> RoutingDetails { get; set; }

        /// <summary>
        /// Array with created routing details (alternative property name from API)
        /// </summary>
        [JsonProperty("routing_details.created")]
        public IReadOnlyList<CreateRoutingDetail> RoutingDetailsCreated { get; set; }

        /// <summary>
        /// Array of cc's emails
        /// </summary>
        [JsonProperty("cc")]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Array of cc's steps
        /// </summary>
        [JsonProperty("cc_step")]
        public IReadOnlyList<CreateRoutingDetailCcStep> CcStep { get; set; }

        /// <summary>
        /// Invite link instruction
        /// </summary>
        [JsonProperty("invite_link_instructions")]
        public string InviteLinkInstructions { get; set; }
    }

    /// <summary>
    /// Create routing detail information
    /// </summary>
    public class CreateRoutingDetail
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
    /// Create routing detail CC step information
    /// </summary>
    public class CreateRoutingDetailCcStep : CcStepBase
    {
    }

}