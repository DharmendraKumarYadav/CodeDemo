using DAL.Models.Idenity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace BDB.Helpers.Common
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, WebApplicationBuilder builder)
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
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        IEmailService _emailer = context.RequestServices.GetService<IEmailService>();

                        var name = builder.Configuration["AppDevName"].ToString();
                        var email = builder.Configuration["AppDevEmail"].ToString();
                        string message = AccountTemplate.SendExceptionEmailAsync(contextFeature.Error.Message.ToString(), contextFeature.Error.StackTrace.ToString());
                        (bool success, string errorMsg) response = await _emailer.SendEmailAsync(name, email, "Exception occured in Dekh Bike Dekh", message);

                        //await context.Response.WriteAsync(new ErrorDetails()
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = "Internal Server Error."
                        //}.ToString());
                        await context.Response.WriteAsync("Internal Server Error.Please try after some time.");
                    }
                });
            });
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
