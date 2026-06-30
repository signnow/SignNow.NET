using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model.Responses
{
    public class GroupInviteStepData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class GroupInviteData : IdResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }

        [JsonProperty("steps")]
        public IReadOnlyList<GroupInviteStepData> Steps { get; set; }
    }

    public class GroupInviteResponse
    {
        [JsonProperty("data")]
        public GroupInviteData Data { get; set; }
    }
}
