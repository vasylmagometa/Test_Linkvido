using Likvido.CreditRisk.Domain.ElasticSearchModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.ElasticSearch.Abstraction
{
    public interface IAnnualReportSearchService
    {
        Task<ElasticAnnualReportModelDTO> FindAnnualReportAsync(string id);

        Task<List<ElasticAnnualReportModelDTO>> FindAnnualReportsAsync(IEnumerable<string> ids);
    }
}
