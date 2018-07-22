using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Models.Credit;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.Services.Abstraction.Scoring;
using System;

namespace Likvido.CreditRisk.Services
{
    public class ScoreService : IScoreService
    {
        private const int DefaultScore = 50;

        private readonly IScoringNumbersService scoringNumbersService;

        private readonly IScoringDescriptionService scoringDescriptionService;

        public ScoreService(IScoringNumbersService scoringNumbersService, IScoringDescriptionService scoringDescriptionService)
        {
            this.scoringNumbersService = scoringNumbersService;
            this.scoringDescriptionService = scoringDescriptionService;
        }

        public Rating GetCompanyRating(Company company)
        {
            ExtendedCompany extendedCompany = company as ExtendedCompany;
            bool isAnnualReportAvailable = company.AnnualReportAvailable;

            decimal ageScore = this.scoringNumbersService.ScoreAge(company.DateStarted);
            decimal employeesScore = this.scoringNumbersService.ScoreEmployees(company.EmployeesNumberForDataModel);
            decimal typeScore = this.scoringNumbersService.ScoreType(company.Type);
            decimal sitautionScore = this.scoringNumbersService.ScoreSituation(company);

            decimal equityScore = 0;
            if (isAnnualReportAvailable && extendedCompany != null)
            {
                equityScore = this.scoringNumbersService.ScoreEquity(extendedCompany.Equity);
            }

            decimal resultScore = 0;
            if (isAnnualReportAvailable && extendedCompany != null)
            {
                resultScore = this.scoringNumbersService.ScoreResult(extendedCompany.ProfitLoss);
            }

            decimal totalScore = ageScore + employeesScore + typeScore + sitautionScore + equityScore + resultScore;

            int summaryRating = CalculateSummaryRating(totalScore);

            Rating rating = new Rating()
            {
                RegistrationNumber = company.VAT,
                SummaryRating = summaryRating,
                Age = new RatingEntity()
                {
                    Value = ageScore,
                    Explaination = this.scoringDescriptionService.DescriptionAge(ageScore),
                    Impact = "2",
                    DataPoint = $"Stiftelsesdato: {company.Startdate}"
                },
                Employees = new RatingEntity()
                {
                    Value = employeesScore,
                    Explaination = this.scoringDescriptionService.DescriptionEmployees(employeesScore),
                    Impact = "1",
                    DataPoint = $"Antal ansatte: {company.EmployeesDescription}"
                },
                Situation = new RatingEntity()
                {
                    Value = sitautionScore,
                    Explaination = this.scoringDescriptionService.DescriptionSituation(sitautionScore),
                    Impact = "3",
                    DataPoint = $"Situation: {company.CompanySituation}"
                },
                Type = new RatingEntity()
                {
                    Value = typeScore,
                    Explaination = this.scoringDescriptionService.DescriptionType(company.Type),
                    Impact = "3",
                    DataPoint = $"Virksomhedsform: {company.CompanyTypeDescription}"
                }
            };

            if (isAnnualReportAvailable && extendedCompany != null)
            {

                rating.Equity = new RatingEntity()
                {
                    Value = equityScore,
                    Explaination = this.scoringDescriptionService.DescriptionEquity(equityScore),
                    Impact = "2",
                    DataPoint = $"Egenkapital: {extendedCompany.Equity}"
                };
                rating.Result = new RatingEntity()
                {
                    Value = resultScore,
                    Explaination = this.scoringDescriptionService.DescriptionResult(resultScore),
                    Impact = "1",
                    DataPoint = $"Resultat: {extendedCompany.ProfitLoss}"
                };
            }

            rating.Stopped = !company.CompanyActive;

            return rating;
        }

        public string MakeGeneralRecommendation(int summaryRating, bool isCompanyActive = true)
        {
            string recommendation = string.Empty;
            if (!isCompanyActive)
            {
                recommendation =
                    "Virksomheden er ophørt eller gået konkurs, hvilket gør at man absolut ikke skal tilbyde denne virksomhed kredit";
            }
            else if (summaryRating <= 2)
            {
                recommendation = "Vi vil ikke anbefale at du giver kredit til denne virksomhed. De har en lav kreditscore, hvilket gør du har større risiko for at miste dine penge";
            }
            else if (summaryRating <= 4)
            {
                recommendation = "Vi vil anbefale at du er påpasselig med at give denne virksomhed kredit. Virksomheden har en kredit indikation i den lave ende, og der kan være risiko for at miste dine penge";
            }
            else if (summaryRating <= 7)
            {
                recommendation = "Vi vil anbefale at du kan give en kort kredit til denne virksomhed, men du stadigvæk bør være påpasselig. Virksomheden har en OK kredit indikation.";
            }
            else
            {
                recommendation = "Vi vil anbefale at du godt kan give virksomheden kredit. Dog bør du altid give den korteste kredit muligt, så du ikke unødvendigt leger bank. Dog har virksomheden en høj kredit indikation.";
            }

            return recommendation;
        }

        public string MakeDebtCollectionRecommendation(int summaryRating, CompanyType companyType, bool isCompanyActive = true)
        {
            string recommendation = string.Empty;
            if (!isCompanyActive)
            {
                if (companyType == CompanyType.Personal 
                    || companyType == CompanyType.IS 
                    || companyType == CompanyType.KS)
                {
                    recommendation =
                        "Virksomheden er desværre ophørt eller gået konkurs. Heldigvis er der personlig hæftelse på kravet, hvilket gør man kan køre en sag mod person(erne) bag virksomheden.";
                }
                else
                {
                    recommendation =
                        "Virksomheden er desvære ophørt eller gået konkurs. Da der ikke er personlig hæftelse på selskabet, er det derfor også tvivlsomt om man kan inddrive noget særligt.";
                }

            }
            else if (summaryRating <= 2)
            {
                recommendation = "Virksomheden har en lav kredit indikation, hvilket gør at vi har ekstra travlt med at få inddrevet dette beløb.";
            }
            else if (summaryRating <= 4)
            {
                recommendation = "Virksomhedens kredit indikation er i den lave ende, hvilket gør at vi arbejder effektivt på at få inddrevet beløbet hurtigst muligt.";
            }
            else if (summaryRating <= 7)
            {
                recommendation = "Virksomheden har en OK kredit indikation, og der er en fin chance for at virksomheden kan betale.";
            }
            else
            {
                recommendation = "Virksomheden har en god kredit indikation. Det gør at chancen for at virksomheden kan betale er høj, og chancen for konkurs er lav.";
            }

            return recommendation;
        }

        public int CalculatePrivateSummaryRating(int numberOfRegistratins)
        {
            var summaryRating = 0;

            if (numberOfRegistratins == 0)
            {
                summaryRating = 10;
            }
            
            if (numberOfRegistratins == 1)
            {
                summaryRating = 3; 
            }

            if (numberOfRegistratins >= 2 && numberOfRegistratins <= 3)
            {
                summaryRating = 2;
            }

            if (numberOfRegistratins >= 4 && numberOfRegistratins <= 7)
            {
                summaryRating = 1;
            }

            return summaryRating;
        }

        private static int CalculateSummaryRating(decimal totalScore)
        {
            decimal factored = (DefaultScore + totalScore) / 10.0m;

            var factoredInt = (int)factored;

            if (factoredInt <= 0)
            {
                factoredInt = 0;
            }

            if (factoredInt >= 10)
            {
                factoredInt = 10;
            }
            
            return factoredInt;
        }
    }
}
