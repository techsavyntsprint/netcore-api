namespace APICore.API.BasicResponses
{
    public class ApiOkResponse : ApiResponse

    {
        public ApiOkResponse(object result)
            : base(200)
        {
            Result = result;
        }

        public object Result { get; }
    }
}