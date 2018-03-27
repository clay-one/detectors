using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleTables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Root.Formatters
{
    public class TableOutputFormatter : OutputFormatter
    {
        public TableOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd+detectors.table"));
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/plain; charset=utf-8";
            base.WriteResponseHeaders(context);
        }

        protected override bool CanWriteType(Type type)
        {
            return GetEnumerableInterfaceType(type) != null;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var enumerableType = GetEnumerableInterfaceType(context.Object.GetType());
            var enumerableArgs = enumerableType.GetGenericArguments();

            var fromOfT = typeof(ConsoleTable).GetMethod(nameof(ConsoleTable.From));
            var from = fromOfT.MakeGenericMethod(enumerableArgs);
            var consoleTable = from.Invoke(null, new[] {context.Object}) as ConsoleTable;

            return context.HttpContext.Response.WriteAsync(consoleTable?.ToString() ?? "");
        }
        
        private static Type GetEnumerableInterfaceType(Type type)
        {
            return type.GetInterfaces().FirstOrDefault(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}