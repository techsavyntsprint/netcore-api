using System.Net;

namespace APICore.Services.Exceptions
{
    public class BaseBadGatewayException : CustomBaseException
    {
        public BaseBadGatewayException() : base()
        {
            HttpCode = (int)HttpStatusCode.BadGateway;
        }
    }
}
