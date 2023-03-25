using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.WebApi.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.WebApi.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new WebApiResult();

            //catches interface because fails on Serializing to JSON ExpectedApiException
            // ( something non-serializable in Excecption class )
            switch (exception)
            {
                case IExpectedApiException expectedApiException:
                    response.StatusCode = HttpStatusCode.OK;
                    response.Error = new
                    {
                        expectedApiException.ErrorCode,
                        expectedApiException.ReasonField,
                        message = expectedApiException.PublicErrorMessage,
                    };
                    break;

                default:
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Error = ErrorCode.Unknown;

                    //todo log error with Logger or Serilog
                    break;
            }

            //
            await response.ExecuteResultAsync(new ActionContext()
            {
                HttpContext = context,
            });
        }
    }
}
