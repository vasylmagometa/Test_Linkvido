using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Services.Abstraction.Scoring;
using System;

namespace Likvido.CreditRisk.Services.Scoring
{
    public class ScoringNumbersService : IScoringNumbersService
    {
        public decimal ScoreAge(DateTime? companyYear)
        {
            var yearToday = DateTime.Now.Year;

            if (!companyYear.HasValue)
            {
                return 0;
            }

            var diff = yearToday - companyYear.Value.Year;

            if (diff >= 0 && diff <= 1)
            {
                return -10;
            }
            if (diff >= 2 && diff <= 3)
            {
                return -5;
            }
            if (diff >= 4 && diff <= 5)
            {
                return 0;
            }
            if (diff >= 6 && diff <= 7)
            {
                return 5;
            }
            if (diff >= 8 && diff <= 10)
            {
                return 10;
            }
            if (diff >= 11 && diff <= 15)
            {
                return 15;
            }
            if (diff >= 16 && diff <= 20)
            {
                return 20;
            }
            if (diff >= 21 && diff <= 25)
            {
                return 25;
            }
            if (diff >= 26)
            {
                return 30;
            }

            return 0;
        }

        public decimal ScoreEmployees(int employeesCount)
        {
            if (employeesCount >= 0 && employeesCount <= 1)
            {
                return -5;
            }
            if (employeesCount >= 2 && employeesCount <= 5)
            {
                return 0;
            }
            if (employeesCount >= 6 && employeesCount <= 10)
            {
                return 5;
            }
            if (employeesCount >= 11 && employeesCount <= 19)
            {
                return 10;
            }
            if (employeesCount >= 20 && employeesCount <= 29)
            {
                return 15;
            }
            if (employeesCount >= 30 && employeesCount <= 50)
            {
                return 20;
            }
            if (employeesCount >= 51 && employeesCount <= 99)
            {
                return 25;
            }
            if (employeesCount >= 100 && employeesCount <= 250)
            {
                return 30;
            }
            if (employeesCount >= 251)
            {
                return 35;
            }

            return 0;
        }

        public decimal ScoreType(CompanyType companyType)
        {
            switch (companyType)
            {
                case CompanyType.APS:
                case CompanyType.Personal:
                    return 0;
                case CompanyType.IS:
                    return 5;
                case CompanyType.AS:
                    return 20;
                case CompanyType.IVS:
                    return -20;
                default:
                    return 0;
            }
        }

        public decimal ScoreSituation(Company company)
        {
            if (company.CompanyStopped
                || company.CompanyDissolved
                || company.CreditBankrupt)
            {
                return -100;
            }

            return 0;
        }

        public decimal ScoreEquity(decimal? equity)
        {
            if (equity >= -999999999 && equity <= -100000)
            {
                return -15;
            }
            // TODO ask -100000
            if (equity >= -100000 && equity <= -1)
            {
                return -10;
            }
            if (equity >= 0 && equity <= 25000)
            {
                return -5;
            }
            if (equity >= 25001 && equity <= 50000)
            {
                return 0;
            }
            if (equity >= 50001 && equity <= 100000)
            {
                return 5;
            }
            if (equity >= 100001 && equity <= 250000)
            {
                return 10;
            }
            // TODO ask -250000
            if (equity >= 250000 && equity <= 500000)
            {
                return 15;
            }
            if (equity >= 500001 && equity <= 1000000)
            {
                return 20;
            }
            if (equity >= 1000001 && equity <= 5000000)
            {
                return 25;
            }
            if (equity >= 5000001 && equity <= 999999999)
            {
                return 30;
            }

            return 0;
        }

        public decimal ScoreResult(decimal? profitLoss)
        {
            // TODO ask about -100000
            if (profitLoss >= -999999999 && profitLoss <= -100000)
            {
                return -5;
            }
            if (profitLoss >= -100000 && profitLoss <= -1)
            {
                return -3;
            }
            if (profitLoss >= 0 && profitLoss <= 25000)
            {
                return 0;
            }
            if (profitLoss >= 25001 && profitLoss <= 50000)
            {
                return 1;
            }
            if (profitLoss >= 50001 && profitLoss <= 100000)
            {
                return 2;
            }
            if (profitLoss >= 100001 && profitLoss <= 250000)
            {
                return 3;
            }
            // TODO ask about 250000
            if (profitLoss >= 250000 && profitLoss <= 500000)
            {
                return 5;
            }
            if (profitLoss >= 500001 && profitLoss <= 1000000)
            {
                return 7;
            }
            if (profitLoss >= 1000001 && profitLoss <= 5000000)
            {
                return 10;
            }
            if (profitLoss >= 5000001 && profitLoss <= 999999999)
            {
                return 15;
            }

            return 0;
        }
    }
}
