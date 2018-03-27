using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Root.Formatters;
using Root.Pipeline;

namespace Root
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.Formatting = Formatting.Indented;
                    json.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                })
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(typeof(FormatFilter));
                    
                    options.OutputFormatters.Add(new BracketsOutputFormatter());
                    options.OutputFormatters.Add(new CsvOutputFormatter());
                    options.OutputFormatters.Add(new DumpOutputFormatter());
                    options.OutputFormatters.Add(new HtmlOutputFormatter());
                    options.OutputFormatters.Add(new JsvOutputFormatter());
                    options.OutputFormatters.Add(new MarkdownOutputFormatter());
                    options.OutputFormatters.Add(new TableOutputFormatter());
                    options.OutputFormatters.Add(new ToStringOutputFormatter());
                    options.OutputFormatters.Add(new XmlOutputFormatter());
                    
                    options.InputFormatters.Add(new XmlSerializerInputFormatter());
                })
                .AddFormatterMappings(mappings =>
                {
                    mappings.SetMediaTypeMappingForFormat("js", "application/json");
                    mappings.SetMediaTypeMappingForFormat("txt", "text/plain");
                    
                    mappings.SetMediaTypeMappingForFormat("brk", "application/vnd+detectors.brackets");
                    mappings.SetMediaTypeMappingForFormat("brackets", "application/vnd+detectors.brackets");
                    mappings.SetMediaTypeMappingForFormat("csv", "application/vnd+detectors.csv");
                    mappings.SetMediaTypeMappingForFormat("dump", "application/vnd+detectors.dump");
                    mappings.SetMediaTypeMappingForFormat("dmp", "application/vnd+detectors.dump");
                    mappings.SetMediaTypeMappingForFormat("htm", "text/html");
                    mappings.SetMediaTypeMappingForFormat("html", "text/html");
                    mappings.SetMediaTypeMappingForFormat("jsv", "application/vnd+detectors.jsv");
                    mappings.SetMediaTypeMappingForFormat("markdown", "application/vnd+detectors.markdown");
                    mappings.SetMediaTypeMappingForFormat("md", "application/vnd+detectors.markdown");
                    mappings.SetMediaTypeMappingForFormat("tbl", "application/vnd+detectors.table");
                    mappings.SetMediaTypeMappingForFormat("table", "application/vnd+detectors.table");
                    mappings.SetMediaTypeMappingForFormat("str", "application/vnd+detectors.string");
                    mappings.SetMediaTypeMappingForFormat("xml", "application/xml");
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MapDeveloperMiddleware(app, env);
            MapWebApi(app);
            MapDefault(app);

            var secondaryApp = app.New();
            MapWebApi(secondaryApp);
            SecondaryPipeline.SecondaryRequestDelegate = secondaryApp.Build();
        }

        private static void MapDeveloperMiddleware(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        private static void MapDefault(IApplicationBuilder app)
        {
            app.Run(context =>
            {
                context.Response.Redirect("/api");
                return Task.CompletedTask;
            });
        }

        private static void MapWebApi(IApplicationBuilder app)
        {
            app.Map("/api", apiApp => { apiApp.UseMvc(); });
        }
    }
}