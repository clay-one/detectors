using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ServiceStack;

namespace Root.Formatters
{
    public class CsvOutputFormatter : OutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd+detectors.csv"));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/plain; charset=utf-8";
            base.WriteResponseHeaders(context);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            return context.HttpContext.Response.WriteAsync(context.Object.ToCsv());
        }
    }
}