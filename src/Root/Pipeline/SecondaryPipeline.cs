using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Root.Pipeline
{
    public static class SecondaryPipeline
    {
        public static RequestDelegate SecondaryRequestDelegate { get; set; }

        public static async Task<SecondatyPipelineInvocationResult> Invoke(
            string uri, string method = "GET", IServiceProvider requestServices = null)
        {
            var responseStream = new MemoryStream();

            HttpContext innerContext = new DefaultHttpContext();
            innerContext.Request.Method = method;
            innerContext.Request.Path = uri;
            innerContext.RequestServices = requestServices;
            innerContext.Response.Body = responseStream;

            await SecondaryRequestDelegate(innerContext);

            return new SecondatyPipelineInvocationResult
            {
                StatusCode = innerContext.Response.StatusCode,
                ResponseBody = Encoding.UTF8.GetString(responseStream.ToArray())
            };
        }
    }
}