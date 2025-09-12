using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response model for getting routing detail information
    /// </summary>
    public class GetRoutingDetailResponse
    {
        /// <summary>
        /// Array with routing details
        /// </summary>
        [JsonProperty("routing_details")]
        public IReadOnlyList<RoutingDetail> RoutingDetails { get; set; }

        /// <summary>
        /// Array of cc's emails
        /// </summary>
        [JsonProperty("cc")]
        public IReadOnlyList<string> Cc { get; set; }

        /// <summary>
        /// Array of cc's steps
        /// </summary>
        [JsonProperty("cc_step")]
        public IReadOnlyList<CcStep> CcStep { get; set; }

        /// <summary>
        /// Invite link instruction
        /// </summary>
        [JsonProperty("invite_link_instructions")]
        public string InviteLinkInstructions { get; set; }

        /// <summary>
        /// Array of viewers
        /// </summary>
        [JsonProperty("viewers")]
        public IReadOnlyList<Viewer> Viewers { get; set; }

        /// <summary>
        /// Array of approvers
        /// </summary>
        [JsonProperty("approvers")]
        public IReadOnlyList<Approver> Approvers { get; set; }

        /// <summary>
        /// Routing attributes
        /// </summary>
        [JsonProperty("attributes")]
        public RoutingAttributes Attributes { get; set; }
    }

    /// <summary>
    /// Routing detail information
    /// </summary>
    public class RoutingDetail
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
        /// Signing order from actor table
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }
    }

    /// <summary>
    /// CC step information
    /// </summary>
    public class CcStep
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

    /// <summary>
    /// Viewer information
    /// </summary>
    public class Viewer
    {
        /// <summary>
        /// Default email for viewer
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Viewer name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signing order
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Contact ID
        /// </summary>
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
    }

    /// <summary>
    /// Approver information
    /// </summary>
    public class Approver
    {
        /// <summary>
        /// Default email for approver
        /// </summary>
        [JsonProperty("default_email")]
        public string DefaultEmail { get; set; }

        /// <summary>
        /// Approver name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Signing order
        /// </summary>
        [JsonProperty("signing_order")]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Always false
        /// </summary>
        [JsonProperty("inviter_role")]
        public bool InviterRole { get; set; }

        /// <summary>
        /// Expiration days
        /// </summary>
        [JsonProperty("expiration_days")]
        public int? ExpirationDays { get; set; }

        /// <summary>
        /// Authentication information
        /// </summary>
        [JsonProperty("authentication")]
        public AuthenticationInfo Authentication { get; set; }
    }

    /// <summary>
    /// Authentication information
    /// </summary>
    public class AuthenticationInfo
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AuthenticationInfoType Type { get; set; }

        /// <summary>
        /// Allowed methods for authentication type phone
        /// </summary>
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneAuthenticationMethod? Method { get; set; }

        /// <summary>
        /// Phone number for authentication type phone
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }
    }

    /// <summary>
    /// Routing attributes
    /// </summary>
    public class RoutingAttributes
    {
        /// <summary>
        /// Brand ID
        /// </summary>
        [JsonProperty("brand_id")]
        public string BrandId { get; set; }

        /// <summary>
        /// Redirect URI
        /// </summary>
        [JsonProperty("redirect_uri")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// On complete action
        /// </summary>
        [JsonProperty("on_complete")]
        public string OnComplete { get; set; }
    }
}