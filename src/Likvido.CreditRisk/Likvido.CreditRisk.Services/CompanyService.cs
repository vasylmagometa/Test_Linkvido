using System.Collections.Generic;
using System.Threading.Tasks;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Services.Factory;
using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.Domain.Models.AnnualReport;
using System.Linq;
using Likvido.CreditRisk.Domain.Exceptions;
using Likvido.CreditRisk.Domain.Models.Credit;

namespace Likvido.CreditRisk.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanySearchService companySearchService;
        
        private readonly IAnnualReportService annualReportService;

        private readonly IScoreService scoreService;

        private readonly ICompanyFactory companyFactory;

        public CompanyService(
            ICompanySearchService companySearchService,
            IAnnualReportService annualReportService,
            IScoreService scoreService,
            ICompanyFactory companyFactory)
        {
            this.companySearchService = companySearchService;
            this.annualReportService = annualReportService;
            this.scoreService = scoreService;
            this.companyFactory = companyFactory;
        }

        public async Task<Company> GetCompanyByIdAsync(string id, RequestType requestType)
        {             
            var companies = await this.GetCompaniesByIdsAsync(new List<string> { id }, requestType);
            var companyById = companies.FirstOrDefault();
            if (companyById == null)
            {
                throw new ResourceNotFoundException($"Company with id {id} was not found.");
            }            
            
            return companyById;
        }

        public async Task<List<Company>> SearchCompaniesAsync(string query, RequestType requestType)
        {
            var companies = await this.companySearchService.SearchCompaniesAsync(query);
            List<AnnualReportXMLData> annualReports = new List<AnnualReportXMLData>();

            if (requestType == RequestType.Extended && companies.Any())
            {
                List<string> foundCompanyIds = companies.Select(c => c.Vrvirksomhed.cvrNummer).ToList();
                annualReports = await this.annualReportService.GetAnnualReports(foundCompanyIds);
            }

            var result = this.GetCompanies(companies, requestType, annualReports);

            return result;
        }

        public async Task<List<Company>> FindCompaniesAsync(List<string> ids, RequestType requestType)
        {
            var companies = await this.companySearchService.FindCompaniesAsync(ids);
            List<AnnualReportXMLData> annualReports = new List<AnnualReportXMLData>();

            if (requestType == RequestType.Extended && companies.Any())
            {
                List<string> foundCompanyIds = companies.Select(c => c.Vrvirksomhed.cvrNummer).ToList();
                annualReports = await this.annualReportService.GetAnnualReports(foundCompanyIds);
            }

            var result = this.GetCompanies(companies, requestType, annualReports);

            return result;
        }

        public async Task<List<CompanyTypeahead>> TypeaheadAsync(string query)
        {
            List<CompanyTypeahead> result = new List<CompanyTypeahead>();
            var companies = await this.companySearchService.SearchCompaniesTypeaheadAsync(query);

            int index = 1;
            foreach (var company in companies)
            {
                string secureTxt = company.Vrvirksomhed.virksomhedMetadata.nyesteNavn.navn.Replace("/", " ");
                string crvNumber = company.Vrvirksomhed.cvrNummer;

                result.Add(new CompanyTypeahead()
                {
                    Id = index,
                    Label = $"{secureTxt} ({crvNumber})",
                    RegistrationName = crvNumber
                });

                index += 1;
            }

            return result;
        }

        public async Task<CreditData> GetCompanyCreditDataByIdAsync(string id)
        {
            Company company = await this.GetCompanyByIdAsync(id, RequestType.Extended);
            CreditData companyCreditData = this.GetCompanyCreditData(company);

            return companyCreditData;
        }

        public async Task<List<CreditData>> FindCompaniesCreditDataAsync(List<string> ids)
        {
            List<Company> companies = await this.FindCompaniesAsync(ids, RequestType.Extended);
            List<CreditData> companiesCreditData = companies.Select(this.GetCompanyCreditData).ToList();

            return companiesCreditData;
        }

        private async Task<List<Company>> GetCompaniesByIdsAsync(List<string> ids, RequestType requestType)
        {
            List<Task> tasks = new List<Task>();
            var companyTask = this.companySearchService.FindCompaniesAsync(ids);
            tasks.Add(companyTask);

            Task<List<AnnualReportXMLData>> annualTask = Task.FromResult(new List<AnnualReportXMLData>());
            if (requestType == RequestType.Extended)
            {
                annualTask = this.annualReportService.GetAnnualReports(ids);
                tasks.Add(annualTask);
            }
            await Task.WhenAll(tasks.ToArray());

            var companies = this.GetCompanies(companyTask.Result, requestType, annualTask.Result);

            return companies;
        }

        private List<Company> GetCompanies(List<ElasticCompanyModelDTO> companies, RequestType requestType, List<AnnualReportXMLData> annualReports)
        {
            List<Company> mergedCompanies = new List<Company>();
            foreach (var company in companies)
            {
                var annualReport = annualReports.Where(c => c != null).FirstOrDefault(c => c.RegistrationNumber == company.Vrvirksomhed.cvrNummer);
                mergedCompanies.Add(this.GetCompany(company, requestType, annualReport));
            }

            return mergedCompanies;
        }

        private Company GetCompany(ElasticCompanyModelDTO company, RequestType requestType, AnnualReportXMLData annualReport)
        {
            Company result = null;
            if (requestType == RequestType.Extended)
            {
                result = this.companyFactory.CreateExtendedCompany(company, annualReport);
            }
            else
            {
                result = this.companyFactory.GetBaseCompany(company);
            }

            return result;
        }

        private CreditData GetCompanyCreditData(Company company)
        {
            Rating rating = this.scoreService.GetCompanyRating(company);

            var result = new CreditData
            {
                Registration = company.VAT,
                RatingData = rating,
                CompanyData = company,
                Recommendation = this.scoreService.MakeGeneralRecommendation(rating.SummaryRating, company.CompanyActive),
                DebtCollectionRecommendation = this.scoreService.MakeDebtCollectionRecommendation(rating.SummaryRating, company.Type, company.CompanyActive)
            };

            return result;
        }
    }
}
