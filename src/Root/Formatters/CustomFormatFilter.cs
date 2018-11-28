using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Root.Formatters
{
    public class CustomFormatFilter : FormatFilter
    {
        public CustomFormatFilter(IOptions<MvcOptions> options, ILoggerFactory loggerFactory) : base(options, loggerFactory)
        {
        }
    }
}