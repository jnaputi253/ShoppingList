using System.Net;

namespace ShoppingListApi.Responses
{
    public class ApiResponse : IApiResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; set; }
        public object Result { get; set; }

        public ApiResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
