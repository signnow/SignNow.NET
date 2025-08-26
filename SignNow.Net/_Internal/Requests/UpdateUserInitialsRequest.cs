using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// Request for updating user initials with image data
    /// </summary>
    internal class UpdateUserInitialsRequest : JsonHttpContent
    {
        /// <summary>
        /// Binary image data for the user's initials.
        /// </summary>
        [JsonProperty("data")]
        [JsonConverter(typeof(StringBase64ToByteArrayJsonConverter))]
        [SuppressMessage("Properties should not return arrays", "CA1819")]
        public byte[] Data { get; set; }
    }
}
