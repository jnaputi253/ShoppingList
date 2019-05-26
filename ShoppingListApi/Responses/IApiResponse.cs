using System.Net;
using Newtonsoft.Json;

namespace ShoppingListApi.Responses
{
    public interface IApiResponse
    {
        HttpStatusCode StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        object Result { get; set; }
    }
}
