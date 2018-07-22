using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Services.Scoring;
using System;
using System.Collections.Generic;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests.Scoring
{
    public class ScoringNumbersServiceTests
    {
        private readonly ScoringNumbersService scoringNumbersService;

        public ScoringNumbersServiceTests()
        {
            this.scoringNumbersService = new ScoringNumbersService();
        }

        [Fact]
        public void ScoreAge_WhenAgeIsNull_Returns_Zero()
        {
            // Arrange
            decimal expected = 0;

            // Act
            var actual = this.scoringNumbersService.ScoreAge(null);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ScoreAge_Returns_Score()
        {
            // Arrange
            List<KeyValuePair<DateTime, decimal>> source = new List<KeyValuePair<DateTime, decimal>>();
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now, -10));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-1), -10));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-2), -5));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-3), -5));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-4), 0));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-5), 0));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-6), 5));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-7), 5));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-8), 10));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-9), 10));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-10), 10));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-11), 15));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-13), 15));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-15), 15));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-16), 20));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-18), 20));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-20), 20));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-21), 25));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-23), 25));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-25), 25));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-26), 30));
            source.Add(new KeyValuePair<DateTime, decimal>(DateTime.Now.AddYears(-70), 30));

            // Act and Assert
            foreach (var item in source)
            {
                var actual = this.scoringNumbersService.ScoreAge(item.Key);
                Assert.Equal(item.Value, actual);
            }
        }

        [Theory]
        [InlineData(0, -5)]
        [InlineData(1, -5)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(5, 0)]
        [InlineData(6, 5)]
        [InlineData(8, 5)]
        [InlineData(10, 5)]
        [InlineData(20, 15)]
        [InlineData(25, 15)]
        [InlineData(29, 15)]
        [InlineData(30, 20)]
        [InlineData(40, 20)]
        [InlineData(50, 20)]
        [InlineData(51, 25)]
        [InlineData(70, 25)]
        [InlineData(99, 25)]
        [InlineData(100, 30)]
        [InlineData(184, 30)]
        [InlineData(250, 30)]
        [InlineData(251, 35)]
        [InlineData(1009, 35)]
        [InlineData(-5, 0)]
        public void ScoreEmployees_Returns_Score(int employeesCount, decimal expected)
        {
            // Act 
            var actual = this.scoringNumbersService.ScoreEmployees(employeesCount);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CompanyType.APS, 0)]
        [InlineData(CompanyType.Personal, 0)]
        [InlineData(CompanyType.IS, 5)]
        [InlineData(CompanyType.AS, 20)]
        [InlineData(CompanyType.IVS, -20)]
        [InlineData(CompanyType.SMBA, 0)]
        [InlineData(CompanyType.SPE, 0)]
        public void ScoreType_Returns_Score(CompanyType companyType, decimal expected)
        {
            // Act 
            var actual = this.scoringNumbersService.ScoreType(companyType);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ScoreSituation_When_CompanyStopped_Returns_Score()
        {
            // Arrenge
            decimal expected = -100;
            Company company = new Company
            {
                CompanyStopped = true,
                CompanyDissolved = false,
                CreditBankrupt = false
            };

            // Act
            var actual = this.scoringNumbersService.ScoreSituation(company);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ScoreSituation_When_CompanyDissolved_Returns_Score()
        {
            // Arrenge
            decimal expected = -100;
            Company company = new Company
            {
                CompanyStopped = false,
                CompanyDissolved = true,
                CreditBankrupt = false
            };

            // Act
            var actual = this.scoringNumbersService.ScoreSituation(company);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ScoreSituation_When_CreditBankrupt_Returns_Score()
        {
            // Arrenge
            decimal expected = -100;
            Company company = new Company
            {
                CompanyStopped = false,
                CompanyDissolved = false,
                CreditBankrupt = true
            };

            // Act
            var actual = this.scoringNumbersService.ScoreSituation(company);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-999999999, -15)]
        [InlineData(-100000, -15)]
        [InlineData(-1, -10)]
        [InlineData(0, -5)]
        [InlineData(25000, -5)]
        [InlineData(25001, 0)]
        [InlineData(50000, 0)]
        [InlineData(50001, 5)]
        [InlineData(100000, 5)]
        [InlineData(100001, 10)]
        [InlineData(250000, 10)]
        [InlineData(500000, 15)]
        [InlineData(500001, 20)]
        [InlineData(1000000, 20)]
        [InlineData(1000001, 25)]
        [InlineData(5000000, 25)]
        [InlineData(5000001, 30)]
        [InlineData(999999999, 30)]
        public void ScoreEquity_Returns_Score(int equity, decimal expected)
        {
            // Act 
            var actual = this.scoringNumbersService.ScoreEquity(equity);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-999999999, -5)]
        [InlineData(-100000, -5)]
        [InlineData(-1, -3)]
        [InlineData(0, 0)]
        [InlineData(25000, 0)]
        [InlineData(25001, 1)]
        [InlineData(50000, 1)]
        [InlineData(50001, 2)]
        [InlineData(100000, 2)]
        [InlineData(100001, 3)]
        [InlineData(250000, 3)]
        [InlineData(500000, 5)]
        [InlineData(500001, 7)]
        [InlineData(1000000, 7)]
        [InlineData(1000001, 10)]
        [InlineData(5000000, 10)]
        [InlineData(5000001, 15)]
        [InlineData(999999999, 15)]
        public void ScoreResult_Returns_Score(int profitLoss, decimal expected)
        {
            // Act 
            var actual = this.scoringNumbersService.ScoreResult(profitLoss);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
