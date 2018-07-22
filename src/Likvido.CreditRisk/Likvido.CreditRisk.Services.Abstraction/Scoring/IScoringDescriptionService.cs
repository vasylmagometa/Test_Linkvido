using Likvido.CreditRisk.Domain.Enums;

namespace Likvido.CreditRisk.Services.Abstraction.Scoring
{
    public interface IScoringDescriptionService
    {
        string DescriptionAge(decimal data);

        string DescriptionEmployees(decimal data);

        string DescriptionType(CompanyType companyType);

        string DescriptionSituation(decimal data);

        string DescriptionEquity(decimal data);

        string DescriptionResult(decimal data);
    }
}
