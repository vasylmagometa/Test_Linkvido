using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Models.Credit;

namespace Likvido.CreditRisk.Services.Abstraction
{
    public interface IScoreService
    {
        Rating GetCompanyRating(Company company);

        string MakeGeneralRecommendation(int summaryRating, bool isCompanyActive = true);

        string MakeDebtCollectionRecommendation(int summaryRating, CompanyType companyType, bool isCompanyActive = true);

        int CalculatePrivateSummaryRating(int numberOfRegistratins);
    }
}
