using Nest;

namespace Likvido.CreditRisk.ElasticSearch
{
    public interface IElasticSearchClientFactory
    {
        IElasticClient GetCompanyElasticClient();

        IElasticClient GetAnnualElasticClient();
    }
}
