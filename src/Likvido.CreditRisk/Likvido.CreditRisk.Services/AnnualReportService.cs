using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.Domain.Models.AnnualReport;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.Services.Factory;
using Likvido.CreditRisk.Services.WebClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Likvido.CreditRisk.Services
{
    public class AnnualReportService : IAnnualReportService
    {
        private readonly IAnnualReportSearchService annualReportSearchService;

        private readonly IWebClientFactory webClientFactory;

        public AnnualReportService(
            IAnnualReportSearchService annualReportSearchService,
            IWebClientFactory webClientFactory)
        {
            this.annualReportSearchService = annualReportSearchService;
            this.webClientFactory = webClientFactory;
        }

        public async Task<AnnualReportXMLData> GetAnnualReport(string id)
        {
            var reports = await this.GetAnnualReports(new List<string> { id });
            
            return reports.FirstOrDefault();
        }

        public async Task<List<AnnualReportXMLData>> GetAnnualReports(IEnumerable<string> ids)
        {
            var reports = await this.annualReportSearchService.FindAnnualReportsAsync(ids);

            var latestReports = reports
                .GroupBy(r => r.cvrNummer)
                .Select(r => r.OrderByDescending(c => c.regnskab?.regnskabsperiode?.slutDato).First())
                .ToList();

            var annualReportXMLDataTasks = latestReports.Select(this.GetAnnualReportData);

            var annualReportXMLData = await Task.WhenAll(annualReportXMLDataTasks);
            
            return annualReportXMLData.ToList();
        }

        private async Task<AnnualReportXMLData> GetAnnualReportData(ElasticAnnualReportModelDTO report)
        {
            var data = await GetAnnualReportDataXml(report);
            
            if (data == null)
            {
                return null;
            }

            var xdoc = XDocument.Parse(data);           
            
            var elements = xdoc?.Root?.Elements();

            if (elements == null || !elements.Any())
            {
                return null;
            }
            
            var equityVal = elements.FirstOrDefault(c => c.Name.LocalName.Equals("Equity"))?.Value;
            var profitLossVal = elements.FirstOrDefault(c => c.Name.LocalName.Equals("ProfitLoss"))?.Value;
            var currentAssetsVal = elements.FirstOrDefault(c => c.Name.LocalName.Equals("CurrentAssets"))?.Value;
            var assetsVal = elements.FirstOrDefault(c => c.Name.LocalName.Equals("Assets"))?.Value;
            var grossProfitLossVal = elements.FirstOrDefault(c => c.Name.LocalName.Equals("GrossProfitLoss"))?.Value;

            var dto = new AnnualReportXMLData()
            {
                RegistrationNumber = report.cvrNummer.ToString(),
                Equity = !string.IsNullOrWhiteSpace(equityVal) ? decimal.Parse(equityVal) : (decimal?)null,
                Assets = !string.IsNullOrWhiteSpace(assetsVal) ? decimal.Parse(assetsVal) : (decimal?)null,
                CurrentAssets = !string.IsNullOrWhiteSpace(currentAssetsVal) ? decimal.Parse(currentAssetsVal) : (decimal?)null,
                ProfitLoss = !string.IsNullOrWhiteSpace(profitLossVal) ? decimal.Parse(profitLossVal) : (decimal?)null,
                GrossProfitLoss = !string.IsNullOrWhiteSpace(grossProfitLossVal) ? decimal.Parse(grossProfitLossVal) : (decimal?)null
            };

            return dto;
        }

        private async Task<string> GetAnnualReportDataXml(ElasticAnnualReportModelDTO report)
        {
            var doc = report?.dokumenter?
                .FirstOrDefault(c => c.dokumentType == "AARSRAPPORT" &&
                                                            c.dokumentMimeType == "application/xml");                                         
            
            if (doc == null)
            {
                return null;
            }            

            using (IWebClient client = this.webClientFactory.GetGzipWebClient())
            {
                client.Encoding = Encoding.UTF8;
                var result = await client.DownloadStringTaskAsync(doc.dokumentUrl);

                return result;
            };
        }
    }
}
