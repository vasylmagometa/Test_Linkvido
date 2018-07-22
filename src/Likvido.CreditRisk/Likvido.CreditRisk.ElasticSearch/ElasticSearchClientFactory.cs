using Likvido.CreditRisk.Infrastructure.Configuration;
using Nest;
using System;

namespace Likvido.CreditRisk.ElasticSearch
{
    public class ElasticSearchClientFactory : IElasticSearchClientFactory
    {
        private readonly IConfigurationManager configurationManager;

        public ElasticSearchClientFactory(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public IElasticClient GetCompanyElasticClient()
        {
            return this.GetElasticClient("Company");
        }

        public IElasticClient GetAnnualElasticClient()
        {
            return this.GetElasticClient("Annual");
        }

        public IElasticClient GetElasticClient(string clusterName)
        {
            var credentials = this.configurationManager.GetElasticConfig(clusterName);
            var settings = new ConnectionSettings(new Uri(credentials.Path))
                .BasicAuthentication(credentials.UserName, credentials.Password)
                .DisableDirectStreaming()
                .PrettyJson();

            return new ElasticClient(settings);
        }
    }
}
