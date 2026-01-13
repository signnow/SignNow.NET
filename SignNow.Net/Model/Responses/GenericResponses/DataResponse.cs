using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses.GenericResponses
{
	public class DataResponse<T>
	{
			[JsonProperty("data")]
			public T Data { get; set; }
	}
}
