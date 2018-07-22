using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Models.Credit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Services.Abstraction
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyByIdAsync(string id, RequestType requestType);

        Task<List<Company>> SearchCompaniesAsync(string query, RequestType requestType);

        Task<List<Company>> FindCompaniesAsync(List<string> ids, RequestType requestType);

        Task<List<CompanyTypeahead>> TypeaheadAsync(string query);

        Task<CreditData> GetCompanyCreditDataByIdAsync(string id);

        Task<List<CreditData>> FindCompaniesCreditDataAsync(List<string> ids);
    }
}
