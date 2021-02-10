namespace APICore.API.BasicResponses
{
    public class ApiCreatedResponse : ApiResponse
    {
        public ApiCreatedResponse(object result)
            : base(201)
        {
            Result = result;
        }

        public object Result { get; }
    }
}