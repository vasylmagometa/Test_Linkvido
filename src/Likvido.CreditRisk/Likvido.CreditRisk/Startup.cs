using AutoMapper;
using Likvido.CreditRisk.Configuration;
using Likvido.CreditRisk.Configuration.Swagger;
using Likvido.CreditRisk.DataAccess.DataContext;
using Likvido.CreditRisk.Domain.Mappers;
using Likvido.CreditRisk.Infrastructure.Configuration;
using Likvido.CreditRisk.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Likvido.CreditRisk
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {   
            this.Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RegistrationProfile).Assembly);
            services.AddMvc();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.ConfigureSwagger();
            services.RegisterServices();

            this.AddDbContext(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.ConfigureSwagger(env);

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc();
        }

        private void AddDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            IConfigurationManager config = serviceProvider.GetService<IConfigurationManager>();
            services.AddDbContext<LikvidoDbContext>(options =>
            options.UseSqlServer(config.LikvidoDatabaseConnectionString));
        }
    }
}
