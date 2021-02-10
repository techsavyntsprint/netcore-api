using Newtonsoft.Json;

namespace APICore.API.BasicResponses
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null, int customStatusCode = 0)
        {
            StatusCode = statusCode;
            Message = message;
            CustomStatusCode = customStatusCode;
        }

        public int StatusCode { get; }
        public int CustomStatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }
    }
}