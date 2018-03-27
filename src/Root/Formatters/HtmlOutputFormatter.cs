using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarkdownLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Root.Formatters
{
    public class HtmlOutputFormatter : OutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/html"));
        }
        
        protected override bool CanWriteType(Type type)
        {
            return GetEnumerableInterfaceType(type) != null;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var enumerableType = GetEnumerableInterfaceType(context.Object.GetType());
            var enumerableArgs = enumerableType.GetGenericArguments();

            var toMarkdownTableOfT = typeof(MarkDownBuilderExtensions)
                .GetMethods()
                .Single(mi => mi.Name == nameof(MarkDownBuilderExtensions.ToMarkdownTable) &&
                              mi.GetParameters().Length == 1);
            
            var toMarkdownTable = toMarkdownTableOfT.MakeGenericMethod(enumerableArgs);
            var table = toMarkdownTable.Invoke(null, new[] {context.Object}) as Table;

            return context.HttpContext.Response.WriteAsync(table?.ToHtml() ?? "");
        }
        
        private static Type GetEnumerableInterfaceType(Type type)
        {
            return type.GetInterfaces().FirstOrDefault(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}