using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.Run(async (context) =>
            {
                var req = context.Request.Path.Value;
                if (req.IndexOf('.') < 0)
                {
                    context.Response.ContentType = "text/html";
                    context.Response.Body.Write(Program.Html, 0, Program.Html.Length);
                }
                else if (req != "/favicon.ico")
                {
                    context.Response.HttpContext.Abort();
                }
            });
        }
    }
}