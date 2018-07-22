using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Services.Scoring;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests.Scoring
{
    public class ScoringDescriptionServiceTests
    {
        private readonly ScoringDescriptionService scoringDescriptionService;

        public ScoringDescriptionServiceTests()
        {
            this.scoringDescriptionService = new ScoringDescriptionService();
        }

        [Theory]
        [InlineData(-1, "Virksomheden er forholdsvis ny, hvilket trækker kredit indikationen ned")]
        [InlineData(10, "Virksomheden har været aktiv i flere år, hvilket giver lidt lavere kredit indikation")]
        [InlineData(12, "Virksomheden har været aktiv i mange år, hvilket løfter kredit indikationen")]
        public void DescriptionAge_Returns_Description(decimal age, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionAge(age);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, "Virksomheden har ingen eller meget få medarbejdere, hvilket trækker kredit indikationen ned")]
        [InlineData(10, "Virksomheden har en del medarbejdere, hvilket giver lavere kredit indikation")]
        [InlineData(12, "Virksomheden har mange medarbejdere, hvilket løfter kredit indikationen")]
        public void DescriptionEmployees_Returns_Description(decimal data, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionEmployees(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CompanyType.APS, "Virksomheden er et anpartsselskab (ApS), hvilket er neutralt")]
        [InlineData(CompanyType.IS, "Virksomheden er et Interessentskab (I/S), hvilket giver lidt kredit indikation, da der er personlig hæftelse over flere personer")]
        [InlineData(CompanyType.AS, "Virksomheden er et aktieselskab (A/S), hvilket giver meget kredit indikation")]
        [InlineData(CompanyType.IVS, "Virksomheden er et iværksætterselskab (IVS), hvilket trækker meget ned i kredit indikationen")]
        [InlineData(CompanyType.Personal, "Virksomheden er personligt ejet, hvilket giver lidt mindre kredit indikation, da der er personlig hæftelse")]
        [InlineData(CompanyType.SMBA, "")]
        public void DescriptionType_Returns_Description(CompanyType data, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionType(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, "Virksomheden er aktiv hvilket er forventeligt")]
        [InlineData(10, "Virksomheden er ikke aktiv (konkurs eller ophørt). Dette gør du IKKE skal handle med virksomheden")]
        [InlineData(-10, "Virksomheden er ikke aktiv (konkurs eller ophørt). Dette gør du IKKE skal handle med virksomheden")]
        public void DescriptionSituation_Returns_Description(decimal data, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionSituation(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, "Virksomhedens egenkapital trækker ned på den samlede kredit indikation")]
        [InlineData(10, "Virksomheden har en acceptabel egenkapital, hvilket giver lidt kredit indikation")]
        [InlineData(12, "Virksomheden har en fin egenkapital, hvilket trækker op i kredit indikationen")]
        public void DescriptionEquity_Returns_Description(decimal data, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionEquity(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, "Virksomhedens seneste årsresultat trækker ned på den samlede kredit indikation")]
        [InlineData(10, "Virksomhedens seneste årsresultat var acceptabelt, hvilket giver lidt kredit indikation")]
        [InlineData(12, "Virksomhedens seneste årsresultat var godt, hvilket trækker op i kredit indikationen")]
        public void DescriptionResult_Returns_Description(decimal data, string expected)
        {
            // Act 
            var actual = this.scoringDescriptionService.DescriptionResult(data);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
