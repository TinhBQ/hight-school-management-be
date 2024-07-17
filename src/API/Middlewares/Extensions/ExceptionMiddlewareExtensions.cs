using DomainModel.Exceptions.BaseExceptions;
using Entities.Exceptions.BaseExceptions;
using Entities.Responses.ErrorResponse;
using Microsoft.AspNetCore.Diagnostics;
using Services.Abstraction.ILoggerServices;
using System.Net;

namespace API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };


                        logger.LogError($"Something went wrong: /n {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }
    }
}
