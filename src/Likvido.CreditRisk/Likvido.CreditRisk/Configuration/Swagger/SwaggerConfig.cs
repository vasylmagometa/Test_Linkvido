using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Likvido.CreditRisk.Configuration.Swagger
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddVersionedApiExplorer(options => { options.SubstituteApiVersionInUrl = true; });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0", new Info { Version = "v1", Title = "Likvido Credit Risk API" });               
            });
        }

        public static void ConfigureSwagger(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/1.0/swagger.json", "Likvido Credit Risk API v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
