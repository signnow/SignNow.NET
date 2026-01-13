using System;
using System.Collections.Generic;
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
    /// Custom HTTP content adapter for create routing detail responses that handles different JSON formats
    /// </summary>
    public class HttpContentToCreateRoutingDetailResponseAdapter : IHttpContentAdapter<CreateRoutingDetailResponse>
    {
        public async Task<CreateRoutingDetailResponse> Adapt(HttpContent content)
        {
            var json = await content.ReadAsStringAsync().ConfigureAwait(false);
            
            try
            {
                // Try to parse as JToken to determine the structure
                var token = JToken.Parse(json);
                
                if (token.Type == JTokenType.Object)
                {
                    var jsonObject = (JObject)token;
                    var response = new CreateRoutingDetailResponse();
                    
                    // Handle routing_details property
                    if (jsonObject["routing_details"] != null)
                    {
                        var routingDetailsToken = jsonObject["routing_details"];
                        if (routingDetailsToken.Type == JTokenType.Array)
                        {
                            response.RoutingDetails = routingDetailsToken.ToObject<IReadOnlyList<CreateRoutingDetail>>();
                        }
                        else if (routingDetailsToken.Type == JTokenType.Object)
                        {
                            // Check if the object has a "data" property containing the array
                            var dataToken = routingDetailsToken["data"];
                            if (dataToken != null && dataToken.Type == JTokenType.Array)
                            {
                                response.RoutingDetails = dataToken.ToObject<IReadOnlyList<CreateRoutingDetail>>();
                            }
                            else
                            {
                                // If it's a single object, wrap it in a list
                                var singleItem = routingDetailsToken.ToObject<CreateRoutingDetail>();
                                response.RoutingDetails = new List<CreateRoutingDetail> { singleItem };
                            }
                        }
                    }
                    
                    // Handle routing_details.created property (alternative format)
                    if (jsonObject["routing_details.created"] != null)
                    {
                        var createdToken = jsonObject["routing_details.created"];
                        if (createdToken.Type == JTokenType.Array)
                        {
                            response.RoutingDetailsCreated = createdToken.ToObject<IReadOnlyList<CreateRoutingDetail>>();
                        }
                        else if (createdToken.Type == JTokenType.Object)
                        {
                            // If it's a single object, wrap it in a list
                            var singleItem = createdToken.ToObject<CreateRoutingDetail>();
                            response.RoutingDetailsCreated = new List<CreateRoutingDetail> { singleItem };
                        }
                        
                        // If we have created details but no regular routing details, use the created ones
                        if (response.RoutingDetails == null)
                        {
                            response.RoutingDetails = response.RoutingDetailsCreated;
                        }
                    }
                    
                    // Handle other properties
                    if (jsonObject["cc"] != null)
                    {
                        response.Cc = jsonObject["cc"].ToObject<IReadOnlyList<string>>();
                    }
                    
                    if (jsonObject["cc_step"] != null)
                    {
                        response.CcStep = jsonObject["cc_step"].ToObject<IReadOnlyList<CreateRoutingDetailCcStep>>();
                    }
                    
                    if (jsonObject["invite_link_instructions"] != null)
                    {
                        response.InviteLinkInstructions = jsonObject["invite_link_instructions"].ToString();
                    }
                    
                    return response;
                }
                else
                {
                    throw new JsonSerializationException($"Unexpected JSON token type: {token.Type}");
                }
            }
            catch (JsonException ex)
            {
                throw new JsonSerializationException($"Failed to deserialize CreateRoutingDetailResponse: {ex.Message}", ex);
            }
        }
    }
}
