using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Root.Formatters
{
    public class PrtgOutputFormatter : OutputFormatter
    {
        public PrtgOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd+detectors.prtg"));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "application/json; charset=utf-8";
            base.WriteResponseHeaders(context);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var type = context.Object.GetType();
            if (GetEnumerableInterfaceType(type) != null)
            {
                var serialized = JsonConvert.SerializeObject(context.Object);
                return context.HttpContext.Response.WriteAsync("{\"prtg\":{\"result\":" + serialized + "}}");
            }
            
            return context.HttpContext.Response.WriteAsync(
                "{\"prtg\":{\"result\":[{\"channel\":\"value\",\"value\":\"" + 
                context.Object + 
                "\"}]}}");
        }
        
        private static Type GetEnumerableInterfaceType(Type type)
        {
            return type.GetInterfaces().FirstOrDefault(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}