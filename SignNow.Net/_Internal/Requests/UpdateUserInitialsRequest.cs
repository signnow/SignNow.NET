using System;
using System.IO;
using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// Request for updating user initials with image data as base64
    /// </summary>
    internal class UpdateUserInitialsRequest : JsonHttpContent
    {
        /// <summary>
        /// Binary image data for the user's initials, encoded as base64 string.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="UpdateUserInitialsRequest"/> with image data stream.
        /// </summary>
        /// <param name="imageData">Stream containing the image data.</param>
        public UpdateUserInitialsRequest(Stream imageData)
        {
            if (imageData == null)
                throw new ArgumentNullException(nameof(imageData));

            using (var memoryStream = new MemoryStream())
            {
                imageData.CopyTo(memoryStream);
                Data = Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}