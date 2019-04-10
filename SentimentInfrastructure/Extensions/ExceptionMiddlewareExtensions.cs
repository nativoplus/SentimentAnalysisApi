using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SentimentViewModels.SentimentMediator;
using System.Net;

namespace SentimentInfrastructure.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    string statusTitle = "Internal Server Error";
                    string errorMessage = contextFeature?.Error.Message;

                    if (contextFeature?.Error.Message == "Bad Request")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        statusTitle = contextFeature?.Error.Message;
                        errorMessage = "The server was unable to process your request due to invalid syntax.";
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }

                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(new SentimentResponse()
                    {
                        Status = statusTitle,
                        Code = context.Response.StatusCode,
                        Message = errorMessage
                    }.ToString());
                });
            });
        }
    }
}
