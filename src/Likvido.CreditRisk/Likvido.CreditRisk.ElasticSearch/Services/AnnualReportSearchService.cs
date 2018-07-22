using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using Likvido.CreditRisk.Domain.ElasticSearchModels;
using System.Threading.Tasks;
using System.Linq;

namespace Likvido.CreditRisk.ElasticSearch.Services
{
    public class AnnualReportSearchService : IAnnualReportSearchService
    {
        private const string NumberFieldName = "cvrNummer";

        private const string AnnualReportIndexName = "offentliggoerelser";

        private readonly IElasticClient elasticClient;

        public AnnualReportSearchService(IElasticSearchClientFactory elasticSearchClientFactory)
        {
            this.elasticClient = elasticSearchClientFactory.GetAnnualElasticClient();
        }

        public async Task<ElasticAnnualReportModelDTO> FindAnnualReportAsync(string id)
        {
            var results = await this.FindAnnualReportsAsync(new List<string> { id });

            return results.FirstOrDefault();
        }
        
        public async Task<List<ElasticAnnualReportModelDTO>> FindAnnualReportsAsync(IEnumerable<string> ids)
        {
            Func<QueryContainerDescriptor<ElasticAnnualReportModelDTO>, QueryContainer> matchQuery =
                qu => ids.Aggregate<string, QueryContainer>(null, (current, id) => current || qu.Term(NumberFieldName, id));
            
            var searchResult = await this.elasticClient.SearchAsync<ElasticAnnualReportModelDTO>(s =>
                s.Index(AnnualReportIndexName).AllTypes().Query(matchQuery));
            
            return searchResult.Documents.ToList();
        }
    }
}
