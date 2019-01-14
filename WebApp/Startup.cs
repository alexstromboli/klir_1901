using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments ("/api"))
                {
                    await next.Invoke();
                    return;
                }

                try
                {
                    HttpWebRequest ProxyRequest = HttpWebRequest.Create("http://localhost:5000" + context.Request.Path) as HttpWebRequest;
                    ProxyRequest.Method = context.Request.Method;
                    ProxyRequest.Headers["Accept"] = context.Request.Headers["Accept"];

                    if (context.Request.Method != "GET")
                    {
                        ProxyRequest.ContentType = context.Request.ContentType;
                        ProxyRequest.ContentLength = context.Request.ContentLength ?? 0;

                        using (Stream ProxyRequestStream = await ProxyRequest.GetRequestStreamAsync ())
                        {
                            await context.Request.Body.CopyToAsync(ProxyRequestStream);
                        }
                    }

                    using (HttpWebResponse ProxyResponse = await ProxyRequest.GetResponseAsync () as HttpWebResponse)
                    {
                        context.Response.StatusCode = (int)ProxyResponse.StatusCode;

                        if ((context.Response.StatusCode / 100) == 2)
                        {
                            context.Response.ContentType = ProxyResponse.ContentType;

                            using (Stream ProxyResponseStream = ProxyResponse.GetResponseStream ())
                            {
                                await ProxyResponseStream.CopyToAsync(context.Response.Body);
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    context.Response.StatusCode = 500;
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
