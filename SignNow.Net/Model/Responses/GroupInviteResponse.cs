using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// A single signing step within a document group invite.
    /// </summary>
    public class GroupInviteStepData
    {
        /// <summary>
        /// Unique identifier of the invite step.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Position of this step in the signing order.
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }

        /// <summary>
        /// Current status of the invite step, e.g. "pending", "fulfilled".
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Details of a document group invite, including its overall status and per-step progress.
    /// </summary>
    public class GroupInviteData : IdResponse
    {
        /// <summary>
        /// Current status of the group invite, e.g. "pending", "fulfilled".
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Date and time when the group invite was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time when the group invite was last updated.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Signing steps that make up this group invite.
        /// </summary>
        [JsonProperty("steps")]
        public IReadOnlyList<GroupInviteStepData> Steps { get; set; }
    }

    /// <summary>
    /// Response returned after creating or retrieving a document group invite.
    /// </summary>
    public class GroupInviteResponse
    {
        /// <summary>
        /// Details of the document group invite.
        /// </summary>
        [JsonProperty("data")]
        public GroupInviteData Data { get; set; }
    }
}
