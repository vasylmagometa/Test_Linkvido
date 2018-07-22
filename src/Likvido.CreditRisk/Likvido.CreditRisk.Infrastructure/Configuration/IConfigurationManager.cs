namespace Likvido.CreditRisk.Infrastructure.Configuration
{
    public interface IConfigurationManager
    {
        string LikvidoDatabaseConnectionString { get; }

        ElaticSearchConfig GetElasticConfig(string clusterName);
    }
}
