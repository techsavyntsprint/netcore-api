using APICore.API.BasicResponses;
using APICore.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace APICore.API.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<ErrorWrappingMiddleware> _localizer;

        public ErrorWrappingMiddleware(RequestDelegate next, IStringLocalizer<ErrorWrappingMiddleware> localizer)
        {
            _next = next;
            _localizer = localizer;
            Message = "";
        }

        private string Message { get; set; }
        private int CustomStatusCode { get; set; }

        public async Task Invoke(HttpContext context)
        {
            Message = "";
            CustomStatusCode = 0;
            try
            {
                await _next.Invoke(context);
            }
            catch (CustomBaseException ex)
            {
                Message = ex.CustomMessage;
                CustomStatusCode = ex.CustomCode;
                context.Response.StatusCode = ex.HttpCode;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                Message = ex.Message;
            }

            if (!context.Response.HasStarted && context.Response.StatusCode != 204)
            {
                context.Response.ContentType = "application/json";

                var response = new ApiResponse(context.Response.StatusCode, Message ?? _localizer.GetString(context.Response.StatusCode.ToString()), CustomStatusCode);

                var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                await context.Response.WriteAsync(json);
            }
        }
    }
}