using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Root.Formatters
{
    public class GigabytesBracketsOutputFormatter : OutputFormatter
    {
        public GigabytesBracketsOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd+detectors.gigabytesbrackets"));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/plain; charset=utf-8";
            base.WriteResponseHeaders(context);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            double.TryParse(context.Object?.ToString(), out var doubleObject);

            doubleObject = doubleObject / 1024 / 1024 / 1024;

            return context.HttpContext.Response.WriteAsync($"[{doubleObject}]");
        }
    }
}