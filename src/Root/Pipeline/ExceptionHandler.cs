using Common;
//using log4net;
using Microsoft.AspNetCore.Http;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace Root.Pipeline
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        //private static readonly ILog Logger = Log4NetHelper.GetLogger(typeof(ExceptionHandler));

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //Logger.Error($"Unhandled exception occured, {nameof(exception.Message)} : {exception.Message}", exception);

            var result = exception.GetChainMessageList().ToJson();
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}