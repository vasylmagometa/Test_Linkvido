using Likvido.Credit.ElasticSearch.Services;
using Likvido.CreditRisk.DataAccess;
using Likvido.CreditRisk.DataAccess.Abstraction;
using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using Likvido.CreditRisk.DataAccess.Repository;
using Likvido.CreditRisk.ElasticSearch;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Likvido.CreditRisk.ElasticSearch.Services;
using Likvido.CreditRisk.Infrastructure.Configuration;
using Likvido.CreditRisk.Services;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.Services.Abstraction.Scoring;
using Likvido.CreditRisk.Services.Factory;
using Likvido.CreditRisk.Services.Scoring;
using Microsoft.Extensions.DependencyInjection;

namespace Likvido.CreditRisk.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddTransient<IConfigurationManager, ConfigurationManager>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IAnnualReportService, AnnualReportService>();
            services.AddTransient<IElasticSearchClientFactory, ElasticSearchClientFactory>();
            services.AddTransient<ICompanySearchService, CompanySearchService>();
            services.AddTransient<IAnnualReportSearchService, AnnualReportSearchService>();            
            services.AddTransient<ICompanyFactory, CompanyFactory>();
            services.AddTransient<IWebClientFactory, WebClientFactory>();
            services.AddTransient<IScoringDescriptionService, ScoringDescriptionService>();
            services.AddTransient<IScoringNumbersService, ScoringNumbersService>(); 
            services.AddTransient<IScoreService, ScoreService>();
            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IRegistrationUserRepository, RegistrationUserRepository>();
            services.AddTransient<ICreditService, CreditService>();
            services.AddTransient<IRegistrationCompanyRepository, RegistrationCompanyRepository>();

            return services;
        }
    }
}
