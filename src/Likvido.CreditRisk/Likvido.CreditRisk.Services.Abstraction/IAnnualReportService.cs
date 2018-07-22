using Likvido.CreditRisk.Domain.Models.AnnualReport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Services.Abstraction
{
    public interface IAnnualReportService
    {
        Task<AnnualReportXMLData> GetAnnualReport(string id);

        Task<List<AnnualReportXMLData>> GetAnnualReports(IEnumerable<string> ids);
    }
}
