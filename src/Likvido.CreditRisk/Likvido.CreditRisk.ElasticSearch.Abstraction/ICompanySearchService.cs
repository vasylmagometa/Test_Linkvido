using Likvido.CreditRisk.Domain.ElasticSearchModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.ElasticSearch.Abstraction
{
    public interface ICompanySearchService
    {
        Task<ElasticCompanyModelDTO> FindCompanyAsync(string id);

        Task<List<ElasticCompanyModelDTO>> FindCompaniesAsync(IEnumerable<string> ids);

        Task<List<ElasticCompanyModelDTO>> SearchCompaniesAsync(string svrNumberOrName);

        Task<List<ElasticCompanyTypeaheadModelDTO>> SearchCompaniesTypeaheadAsync(string svrNumberOrName);

    }
}
