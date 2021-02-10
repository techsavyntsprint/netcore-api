namespace APICore.API.BasicResponses
{
    public class ApiServiceUnavailableResponse : ApiResponse
    {
        public ApiServiceUnavailableResponse(object result)
           : base(500)
        {
            Result = result;
        }

        public object Result { get; }
    }
}