using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImageUploader
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
            services.AddControllers();
            //todo-added cors- this order NOT matters 1
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseHsts();
                app.UseDeveloperExceptionPage();
            }
            //todo-put this else 4
            else
            {
                app.UseHsts();
                //todo-step 5 add new  folder called "wwwroot" by right clicking the project
                //todo-step 6 final - go properties>launch setting change weatherforcast to "api/ImageUpload" (has 2 places to be changed)
            }

            //todo-added cors- this order matters 2
            app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //todo-put this 2
            app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //todo-put this 3
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Could Not Find Anything");
            });
        }
    }
}
