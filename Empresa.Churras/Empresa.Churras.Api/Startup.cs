using Empresa.Churras.Infra;
using Kernel.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

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

            IoC.Container.Verify();
        }
    }
}
