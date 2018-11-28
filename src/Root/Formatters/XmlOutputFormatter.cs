using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ServiceStack;

namespace Root.Formatters
{
    public class XmlOutputFormatter : OutputFormatter
    {
        public XmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            return context.HttpContext.Response.WriteAsync(context.Object.ToXml());
        }
    }
}