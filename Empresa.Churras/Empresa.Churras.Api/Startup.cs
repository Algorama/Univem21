using Empresa.Churras.Infra;
using Kernel.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Empresa.Churras.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            IoC.InitializeContainer();

            // https://simpleinjector.readthedocs.io/en/latest/aspnetintegration.html
            services.AddSimpleInjector(IoC.Container, options =>
            {
                options.AddAspNetCore()
                       .AddControllerActivation();
            });
            
            IoC.Start<ChurrasContext>();

            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Churras API", Version = "v1" });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjector(IoC.Container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Churras API V1");
            });
        }
    }
}
