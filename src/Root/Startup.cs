using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();
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