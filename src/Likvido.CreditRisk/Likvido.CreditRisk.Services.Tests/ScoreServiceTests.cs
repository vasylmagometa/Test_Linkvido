using AutoFixture;
using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Models.Credit;
using Likvido.CreditRisk.Services.Abstraction.Scoring;
using Moq;
using System;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests
{
    public class ScoreServiceTests
    {
        #region Constants

        private const int ExpectedAgeScore = 5;
        private const int ExpectedEmployeesScore = 10;
        private const int ExpectedEquityScore = 15;
        private const int ExpectedResultScore = -3;
        private const int ExpectedSituationScore = 0;
        private const int ExpectedTypeScore = 20;

        private const string ExpectedAgeDescription = "Age";
        private const string ExpectedEmployeesDescription = "Employees";
        private const string ExpectedEquityDescription = "Equity";
        private const string ExpectedResultDescription = "Result";
        private const string ExpectedSituationDescription = "Situation";
        private const string ExpectedTypeDescription = "Type";

        #endregion

        private readonly ScoreService scoreService;

        private readonly Mock<IScoringNumbersService> scoringNumbersServiceMock = new Mock<IScoringNumbersService>();

        private readonly Mock<IScoringDescriptionService> scoringDescriptionServiceMock = new Mock<IScoringDescriptionService>();

        public ScoreServiceTests()
        {
            this.scoreService = new ScoreService(scoringNumbersServiceMock.Object, scoringDescriptionServiceMock.Object);
            this.StubScoringNumbersService();
            this.StubSscoringDescriptionService();
        }

        [Fact]
        public void GetCompanyRating_BaseCompany_Returns_Rating()
        {
            // Arange 
            this.StubScoringNumbersService();
            Fixture fixture = new Fixture();
            Company company = this.CreateBaseCompany(fixture);

            int expectedSummaryRating = 8;

            // Act 
            var actual = this.scoreService.GetCompanyRating(company);

            // Assert
            this.AssertBaseCompanyRaping(actual, company, expectedSummaryRating);
            Assert.Null(actual.Equity);
            Assert.Null(actual.Result);
        }

        [Fact]
        public void GetCompanyRating_ExtendedCompany_Returns_Rating()
        {
            // Arange 
            Fixture fixture = new Fixture();
            ExtendedCompany company = this.CreateExtendedCompany(fixture);

            int expectedSummaryRating = 9;

            // Act 
            var actual = this.scoreService.GetCompanyRating(company);

            // Assert
            this.AssertBaseCompanyRaping(actual, company, expectedSummaryRating);
            AssertRatingEntity(actual.Equity, ExpectedEquityScore, ExpectedEquityDescription, "2", $"Egenkapital: {company.Equity}");
            AssertRatingEntity(actual.Result, ExpectedResultScore, ExpectedResultDescription, "1", $"Resultat: {company.ProfitLoss}");
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 3)]
        [InlineData(2, 2)]
        [InlineData(3, 2)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        [InlineData(7, 1)]
        [InlineData(100, 0)]
        public void CalculatePrivateSummaryRating_Returs_SummaryRating(int numberOfRegistratins, int expected)
        {
            // Act 
            var actual = this.scoreService.CalculatePrivateSummaryRating(numberOfRegistratins);

            // Assert
            Assert.Equal(expected, actual);
        }

        #region MakeGeneralRecommendation

        [Fact]
        public void MakeGeneralRecommendation_RetursRecommendation_When_CompanyIsNotActive()
        {
            // Arrange
            Fixture fixture = new Fixture();
            int summaryRating = fixture.Create<int>();
            string expected = "Virksomheden er ophørt eller gået konkurs, hvilket gør at man absolut ikke skal tilbyde denne virksomhed kredit"; ;

            // Act 
            var actual = this.scoreService.MakeGeneralRecommendation(summaryRating, false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void MakeGeneralRecommendation_RetursRecommendation_When_RatingLessOrEqual_2(int summaryRating)
        {
            // Arrange
            string expected = "Vi vil ikke anbefale at du giver kredit til denne virksomhed. De har en lav kreditscore, hvilket gør du har større risiko for at miste dine penge";

            // Act 
            var actual = this.scoreService.MakeGeneralRecommendation(summaryRating);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void MakeGeneralRecommendation_RetursRecommendation_When_RatingLessOrEqual_4(int summaryRating)
        {
            // Arrange
            string expected = "Vi vil anbefale at du er påpasselig med at give denne virksomhed kredit. Virksomheden har en kredit indikation i den lave ende, og der kan være risiko for at miste dine penge";

            // Act 
            var actual = this.scoreService.MakeGeneralRecommendation(summaryRating);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        public void MakeGeneralRecommendation_RetursRecommendation_When_RatingLessOrEqual_7(int summaryRating)
        {
            // Arrange
            string expected = "Vi vil anbefale at du kan give en kort kredit til denne virksomhed, men du stadigvæk bør være påpasselig. Virksomheden har en OK kredit indikation.";

            // Act 
            var actual = this.scoreService.MakeGeneralRecommendation(summaryRating);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(100)]
        public void MakeGeneralRecommendation_RetursRecommendation_When_RatingMore_7(int summaryRating)
        {
            // Arrange
            string expected = "Vi vil anbefale at du godt kan give virksomheden kredit. Dog bør du altid give den korteste kredit muligt, så du ikke unødvendigt leger bank. Dog har virksomheden en høj kredit indikation.";

            // Act 
            var actual = this.scoreService.MakeGeneralRecommendation(summaryRating);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region MakeDebtCollectionRecommendation

        [Fact]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_CompanyIsNotActive()
        {
            // Arrange
            Fixture fixture = new Fixture();
            int summaryRating = fixture.Create<int>();
            string expected = "Virksomheden er desvære ophørt eller gået konkurs. Da der ikke er personlig hæftelse på selskabet, er det derfor også tvivlsomt om man kan inddrive noget særligt.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, CompanyType.SMBA, false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CompanyType.Personal)]
        [InlineData(CompanyType.IS)]
        [InlineData(CompanyType.KS)]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_CompanyIsNotActive_AndHasSpecificType(CompanyType companyType)
        {
            // Arrange
            Fixture fixture = new Fixture();
            int summaryRating = fixture.Create<int>();
            string expected = "Virksomheden er desværre ophørt eller gået konkurs. Heldigvis er der personlig hæftelse på kravet, hvilket gør man kan køre en sag mod person(erne) bag virksomheden.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, companyType, false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_RatingLessOrEqual_2(int summaryRating)
        {
            // Arrange
            string expected = "Virksomheden har en lav kredit indikation, hvilket gør at vi har ekstra travlt med at få inddrevet dette beløb.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, CompanyType.APS);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_RatingLessOrEqual_4(int summaryRating)
        {
            // Arrange
            string expected = "Virksomhedens kredit indikation er i den lave ende, hvilket gør at vi arbejder effektivt på at få inddrevet beløbet hurtigst muligt.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, CompanyType.APS);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(7)]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_RatingLessOrEqual_7(int summaryRating)
        {
            // Arrange
            string expected = "Virksomheden har en OK kredit indikation, og der er en fin chance for at virksomheden kan betale.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, CompanyType.APS);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(100)]
        public void MakeDebtCollectionRecommendation_RetursRecommendation_When_RatingMore_7(int summaryRating)
        {
            // Arrange
            string expected = "Virksomheden har en god kredit indikation. Det gør at chancen for at virksomheden kan betale er høj, og chancen for konkurs er lav.";

            // Act 
            var actual = this.scoreService.MakeDebtCollectionRecommendation(summaryRating, CompanyType.APS);

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        private void StubScoringNumbersService()
        {
            this.scoringNumbersServiceMock.Setup(s => s.ScoreAge(It.IsAny<DateTime?>())).Returns(ExpectedAgeScore);
            this.scoringNumbersServiceMock.Setup(s => s.ScoreEmployees(It.IsAny<int>())).Returns(ExpectedEmployeesScore);
            this.scoringNumbersServiceMock.Setup(s => s.ScoreEquity(It.IsAny<decimal?>())).Returns(ExpectedEquityScore);
            this.scoringNumbersServiceMock.Setup(s => s.ScoreResult(It.IsAny<decimal?>())).Returns(ExpectedResultScore);
            this.scoringNumbersServiceMock.Setup(s => s.ScoreSituation(It.IsAny<Company>())).Returns(ExpectedSituationScore);
            this.scoringNumbersServiceMock.Setup(s => s.ScoreType(It.IsAny<CompanyType>())).Returns(ExpectedTypeScore);
        }

        private void StubSscoringDescriptionService()
        {
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionAge(It.IsAny<decimal>())).Returns(ExpectedAgeDescription);
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionEmployees(It.IsAny<decimal>())).Returns(ExpectedEmployeesDescription);
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionEquity(It.IsAny<decimal>())).Returns(ExpectedEquityDescription);
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionResult(It.IsAny<decimal>())).Returns(ExpectedResultDescription);
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionSituation(It.IsAny<decimal>())).Returns(ExpectedSituationDescription);
            this.scoringDescriptionServiceMock.Setup(s => s.DescriptionType(It.IsAny<CompanyType>())).Returns(ExpectedTypeDescription);
        }

        private Company CreateBaseCompany(Fixture fixture)
        {
            return fixture
                .Build<Company>()
                .With(c => c.AnnualReportAvailable, false)
                .Create();
        }

        private ExtendedCompany CreateExtendedCompany(Fixture fixture)
        {
            return fixture
                .Build<ExtendedCompany>()
                .With(c => c.AnnualReportAvailable, true)
                .Create();
        }

        private void AssertBaseCompanyRaping(Rating actual, Company company, int summaryRating)
        {
            Assert.Equal(summaryRating, actual.SummaryRating);
            Assert.Equal(company.VAT, actual.RegistrationNumber);
            Assert.Equal(!company.CompanyActive, actual.Stopped);

            AssertRatingEntity(actual.Age, ExpectedAgeScore, ExpectedAgeDescription, "2", $"Stiftelsesdato: {company.Startdate}");
            AssertRatingEntity(actual.Employees, ExpectedEmployeesScore, ExpectedEmployeesDescription, "1", $"Antal ansatte: {company.EmployeesDescription}");
            AssertRatingEntity(actual.Situation, ExpectedSituationScore, ExpectedSituationDescription, "3", $"Situation: {company.CompanySituation}");
            AssertRatingEntity(actual.Type, ExpectedTypeScore, ExpectedTypeDescription, "3", $"Virksomhedsform: {company.CompanyTypeDescription}");
        }

        private void AssertRatingEntity(RatingEntity actual, decimal? value, string explaination, string impact, string dataPoint)
        {
            Assert.Equal(value, actual.Value);
            Assert.Equal(explaination, actual.Explaination);
            Assert.Equal(impact, actual.Impact);
            Assert.Equal(dataPoint, actual.DataPoint);
        }
    }
}
