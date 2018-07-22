using AutoFixture;
using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Likvido.CreditRisk.Services.Factory;
using Likvido.CreditRisk.Services.WebClient;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests
{
    public class AnnualReportServiceTests
    {
        private readonly AnnualReportService annualReportService;

        private readonly Mock<IAnnualReportSearchService> annualReportSearchServiceMock = new Mock<IAnnualReportSearchService>();

        private readonly Mock<IWebClientFactory> webClientFactoryMock = new Mock<IWebClientFactory>();

        private readonly Mock<IWebClient> webClientMock = new Mock<IWebClient>();

        public AnnualReportServiceTests()
        {
            this.annualReportService = new AnnualReportService(this.annualReportSearchServiceMock.Object, this.webClientFactoryMock.Object);
            this.webClientFactoryMock.Setup(f => f.GetGzipWebClient()).Returns(this.webClientMock.Object);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_Null_When_CompanyNotFound()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };
            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(new List<ElasticAnnualReportModelDTO>()));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_Null_When_dokumentUrlNull()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var annualreports = this.CreateElasticAnnualReportModelDTOWithoutUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_Null_When_XmlDataNull()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var annualreports = this.CreateElasticAnnualReportModelDTOWithUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_Null_When_XmlDataEmpty()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var annualreports = this.CreateElasticAnnualReportModelDTOWithUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            this.webClientMock.Setup(w => w.DownloadStringTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(CreateXmlData(new List<KeyValuePair<string, string>>())));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_EmpatyValues_When_XmlDataDoeNotContainData()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var annualreports = this.CreateElasticAnnualReportModelDTOWithUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            this.webClientMock.Setup(w => w.DownloadStringTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(CreateXmlData(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("test", "tets") })));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Null(actual.Assets);
            Assert.Null(actual.CurrentAssets);
            Assert.Null(actual.GrossProfitLoss);
            Assert.Null(actual.ProfitLoss);
            Assert.Null(actual.Equity);
            Assert.Equal(companyId, actual.RegistrationNumber);
        }

        [Fact]
        public async Task GetAnnualReport_Retuns_CorrectReport()
        {
            // Arrange
            Fixture fixture = new Fixture();

            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var annualreports = this.CreateElasticAnnualReportModelDTOWithUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            var properties = new Dictionary<string, string>();
            properties.Add("Assets", fixture.Create<decimal>().ToString());
            properties.Add("CurrentAssets", fixture.Create<decimal>().ToString());
            properties.Add("GrossProfitLoss", fixture.Create<decimal>().ToString());
            properties.Add("ProfitLoss", fixture.Create<decimal>().ToString());
            properties.Add("Equity", fixture.Create<decimal>().ToString());

            this.webClientMock.Setup(w => w.DownloadStringTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(CreateXmlData(properties)));

            // Act
            var actual = await annualReportService.GetAnnualReport(companyId);

            // Assert
            Assert.Equal(decimal.Parse(properties["Assets"]), actual.Assets);
            Assert.Equal(decimal.Parse(properties["CurrentAssets"]), actual.CurrentAssets);
            Assert.Equal(decimal.Parse(properties["GrossProfitLoss"]), actual.GrossProfitLoss);
            Assert.Equal(decimal.Parse(properties["ProfitLoss"]), actual.ProfitLoss);
            Assert.Equal(decimal.Parse(properties["Equity"]), actual.Equity);
            Assert.Equal(companyId, actual.RegistrationNumber);
        }

        [Fact]
        public async Task GetAnnualReports_Retuns_CompanyReports()
        {
            // Arrange
            Fixture fixture = new Fixture();

            List<string> companyIds = fixture.CreateMany<string>(3).ToList();

            var annualreports = this.CreateElasticAnnualReportModelDTOWithUrls(fixture, companyIds);

            this.annualReportSearchServiceMock.Setup(s => s.FindAnnualReportsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(annualreports));

            var properties = new Dictionary<string, string>();
            properties.Add("Assets", fixture.Create<decimal>().ToString());

            this.webClientMock.Setup(w => w.DownloadStringTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(CreateXmlData(properties)));

            // Act
            var actual = await annualReportService.GetAnnualReports(companyIds);

            // Assert
            Assert.Equal(companyIds, actual.Select(a => a.RegistrationNumber));
        }

        private List<ElasticAnnualReportModelDTO> CreateElasticAnnualReportModelDTOWithoutUrls(Fixture fixture, List<string> companiesSrv)
        {
            List<ElasticAnnualReportModelDTO> anualReports = new List<ElasticAnnualReportModelDTO>();

            foreach (var companySrv in companiesSrv)
            {
                anualReports.Add(fixture
                    .Build<ElasticAnnualReportModelDTO>()
                    .With(e => e.cvrNummer, companySrv)
                    .Create());
            }

            return anualReports;
        }

        private List<ElasticAnnualReportModelDTO> CreateElasticAnnualReportModelDTOWithUrls(Fixture fixture, List<string> companiesSrv)
        {
            List<ElasticAnnualReportModelDTO> anualReports = this.CreateElasticAnnualReportModelDTOWithoutUrls(fixture, companiesSrv);

            foreach (var report in anualReports)
            {
                report.dokumenter[0].dokumentType = "AARSRAPPORT";
                report.dokumenter[0].dokumentMimeType = "application/xml";
            }

            return anualReports;
        }

        private string CreateXmlData(IEnumerable<KeyValuePair<string, string>> elements)
        {
            XDocument document = new XDocument();
            document.Add(new XElement("Root", "content"));
            foreach (var element in elements)
            {
                document.Root.Add(new XElement(element.Key, element.Value));
            }

            return document.ToString();
        }
    }
}
