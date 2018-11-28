using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Root.Formatters
{
    public class BracketsOutputFormatter : OutputFormatter
    {
        public BracketsOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd+detectors.brackets"));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/plain; charset=utf-8";
            base.WriteResponseHeaders(context);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            return context.HttpContext.Response.WriteAsync($"[{context.Object}]");
        }
    }
}