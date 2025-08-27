using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        public byte[] Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserInitialsRequest"/> class.
        /// </summary>
        /// <param name="imageData">The image data stream to be processed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        public static async Task<UpdateUserInitialsRequest> CreateAsync(Stream imageData, CancellationToken cancellationToken = default)
        {
            if (imageData == null)
                throw new ArgumentNullException(nameof(imageData));

            var request = new UpdateUserInitialsRequest();
            await request.ProcessImageDataAsync(imageData, cancellationToken).ConfigureAwait(false);
            return request;
        }

        /// <summary>
        /// Private constructor to ensure async initialization through CreateAsync method.
        /// </summary>
        private UpdateUserInitialsRequest()
        {
        }

        /// <summary>
        /// Processes the image data stream and converts it to byte array.
        /// </summary>
        /// <param name="imageData">The image data stream.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        private async Task ProcessImageDataAsync(Stream imageData, CancellationToken cancellationToken)
        {
            using (var memoryStream = new MemoryStream())
            {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
                await imageData.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
#else
                await imageData.CopyToAsync(memoryStream).ConfigureAwait(false);
#endif
                Data = memoryStream.ToArray();
            }
        }
    }
}
