using Microsoft.Extensions.Configuration;

namespace Likvido.CreditRisk.Infrastructure.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string LikvidoDatabaseConnectionString => this.GetConnectionStringValue("LikvidoDatabaseConnectionString");

        public ElaticSearchConfig GetElasticConfig(string clusterName)
        {
            var path = $"Elastic:{clusterName}";
            var credentials = new ElaticSearchConfig
            {
                UserName = this.configuration[$"{path}:UserName"],
                Password = this.configuration[$"{path}:Password"],
                Path = this.configuration[$"{path}:Path"]
            };

            return credentials;
        }

        public string GetConnectionStringValue(string connStringName)
        {
            return this.configuration.GetConnectionString(connStringName);
        }
    }
}
