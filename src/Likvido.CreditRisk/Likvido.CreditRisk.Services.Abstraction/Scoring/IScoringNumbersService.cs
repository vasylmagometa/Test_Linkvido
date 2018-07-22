using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using System;

namespace Likvido.CreditRisk.Services.Abstraction.Scoring
{
    public interface IScoringNumbersService
    {
        decimal ScoreAge(DateTime? companyYear);

        decimal ScoreEmployees(int employeesCount);

        decimal ScoreType(CompanyType companyType);

        decimal ScoreSituation(Company company);

        decimal ScoreEquity(decimal? equity);

        decimal ScoreResult(decimal? profitLoss);
    }
}
