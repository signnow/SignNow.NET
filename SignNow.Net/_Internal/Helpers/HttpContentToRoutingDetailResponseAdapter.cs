using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Internal.Helpers
{
    /// <summary>
    /// Custom HTTP content adapter for routing detail responses that handles different JSON formats
    /// </summary>
    public class HttpContentToRoutingDetailResponseAdapter : IHttpContentAdapter<GetRoutingDetailResponse>
    {
        public async Task<GetRoutingDetailResponse> Adapt(HttpContent content)
        {
            var json = await content.ReadAsStringAsync().ConfigureAwait(false);
            
            try
            {
                // Try to parse as JToken to determine the structure
                var token = JToken.Parse(json);
                
                if (token.Type == JTokenType.Array)
                {
                    // If the response is an array, create a response with default values
                    return new GetRoutingDetailResponse
                    {
                        InviteLinkInstructions = string.Empty
                    };
                }
                else if (token.Type == JTokenType.Object)
                {
                    // If the response is an object, deserialize normally
                    return JsonConvert.DeserializeObject<GetRoutingDetailResponse>(json);
                }
                else
                {
                    throw new JsonSerializationException($"Unexpected JSON token type: {token.Type}");
                }
            }
            catch (JsonException ex)
            {
                throw new JsonSerializationException($"Failed to deserialize GetRoutingDetailResponse: {ex.Message}", ex);
            }
        }
    }
}
