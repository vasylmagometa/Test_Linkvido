using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.ElasticSearch;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Likvido.Credit.ElasticSearch.Services
{
    public class CompanySearchService : ICompanySearchService
    {
        private const string NumberFieldName = "Vrvirksomhed.cvrNummer";

        private const string NameFieldName = "Vrvirksomhed.virksomhedMetadata.nyesteNavn.navn";

        private const string CompanyIndexName = "cvr-permanent";

        private readonly IElasticClient elasticClient;

        public CompanySearchService(IElasticSearchClientFactory elasticSearchClientFactory)
        {
            this.elasticClient = elasticSearchClientFactory.GetCompanyElasticClient();
        }

        public async Task<ElasticCompanyModelDTO> FindCompanyAsync(string id)
        {
            var results = await this.FindCompaniesAsync(new List<string> { id });

            return results.FirstOrDefault();
        }

        public Task<List<ElasticCompanyModelDTO>> FindCompaniesAsync(IEnumerable<string> ids)
        {
            Func<QueryContainerDescriptor<ElasticCompanyModelDTO>, QueryContainer> matchQuery = 
                qu => ids.Aggregate<string, QueryContainer>(null, (current, id) => current || qu.Term(NumberFieldName, id));

            return this.SearchAsync<ElasticCompanyModelDTO>(matchQuery);
        }        

        public Task<List<ElasticCompanyModelDTO>> SearchCompaniesAsync(string svrNumberOrName)
        {            
            return this.SearchAsync<ElasticCompanyModelDTO>(q => q.MatchPhrasePrefix(ms => ms.Field(NameFieldName).Query(svrNumberOrName))
                    || q.MatchPhrasePrefix((ms => ms.Field(NumberFieldName).Query(svrNumberOrName))));                
        }

        public Task<List<ElasticCompanyTypeaheadModelDTO>> SearchCompaniesTypeaheadAsync(string svrNumberOrName)
        {            
            return this.SearchAsync<ElasticCompanyTypeaheadModelDTO>(q => q.MatchPhrasePrefix(ms => ms.Field(NameFieldName).Query(svrNumberOrName))
                    || q.MatchPhrasePrefix((ms => ms.Field(NumberFieldName).Query(svrNumberOrName))));
        }

        private async Task<List<T>> SearchAsync<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query) where T : class
        {
            var searchResult = await this.elasticClient.SearchAsync<T>(s => s.Index(CompanyIndexName).AllTypes().Query(query));

            return searchResult.Documents.ToList();
        }
    }
}
