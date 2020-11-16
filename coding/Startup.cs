using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
namespace server
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                var req = context.Request.Path.Value;
                if (req.IndexOf('.') < 0)
                {
                    context.Response.ContentType = "text/html";
                    return context.Response.Body.WriteAsync(Program.Html, 0, Program.Html.Length);
                }
                else
                {
                    return next();
                }
            });
            app.UseStaticFiles();
        }
    }
}