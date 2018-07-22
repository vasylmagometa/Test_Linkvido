using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.Domain.Models.AnnualReport;
using Likvido.CreditRisk.Domain.Models.CompanyModels;

namespace Likvido.CreditRisk.Services.Factory
{
    public interface ICompanyFactory
    {
        Company GetBaseCompany(ElasticCompanyModelDTO model);

        ExtendedCompany CreateExtendedCompany(ElasticCompanyModelDTO model, AnnualReportXMLData annualReport);
    }
}
