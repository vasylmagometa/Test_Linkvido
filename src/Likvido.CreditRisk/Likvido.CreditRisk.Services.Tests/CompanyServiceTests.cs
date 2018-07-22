using AutoFixture;
using Likvido.CreditRisk.Domain.ElasticSearchModels;
using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Domain.Exceptions;
using Likvido.CreditRisk.Domain.Models.AnnualReport;
using Likvido.CreditRisk.Domain.Models.CompanyModels;
using Likvido.CreditRisk.Domain.Models.Credit;
using Likvido.CreditRisk.ElasticSearch.Abstraction;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.Services.Factory;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests
{
    public class CompanyServiceTests
    {
        private readonly CompanyService companyService;

        private readonly Mock<ICompanySearchService> companySearchServiceMock = new Mock<ICompanySearchService>();

        private readonly Mock<IAnnualReportService> annualReportServiceMock = new Mock<IAnnualReportService>();

        private readonly Mock<IScoreService> scoreServiceMock = new Mock<IScoreService>();

        public CompanyServiceTests()
        {
            this.companyService = new CompanyService(companySearchServiceMock.Object, annualReportServiceMock.Object, scoreServiceMock.Object, new CompanyFactory());
        }

        [Fact]
        public void GetCompanyByIdAsync_ThrowException_WhenNothingFound()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };
            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(new List<ElasticCompanyModelDTO>()));

            // Act
            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await this.companyService.GetCompanyByIdAsync(companyId, RequestType.Ligth));
            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_Returns_FoundCustomer()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };
            var companies = CreateElastincCompanyModels(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            // Act
            var actual = await this.companyService.GetCompanyByIdAsync(companyId, RequestType.Ligth);

            // Assert
            Assert.Equal(companies[0].Vrvirksomhed.cvrNummer, actual.VAT);
            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
        }

        [Fact]
        public async Task FindCompaniesAsync_RequestType_Light_Returns_FoundCustomers()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();

            var companies = CreateElastincCompanyModels(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            // Act
            var actual = await this.companyService.FindCompaniesAsync(companyIds, RequestType.Ligth);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
        }

        [Fact]
        public async Task FindCompaniesAsync_RequestType_Extended_Returns_FoundCustomers()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            // Act
            var actual = await this.companyService.FindCompaniesAsync(companyIds, RequestType.Extended);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            Assert.True(actual.All(r => r.AnnualReportAvailable));
            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
        }

        [Fact]
        public async Task FindCompaniesAsync_RequestType_Extended_AunalDoesNotExist_Returns_FoundCustomersWOAnualReposts()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, fixture.CreateMany<string>(3).ToList());

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            // Act
            var actual = await this.companyService.FindCompaniesAsync(companyIds, RequestType.Extended);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            Assert.False(actual.All(r => r.AnnualReportAvailable));
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Once);
            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
        }

        [Fact]
        public async Task FindCompaniesAsync_RequestType_Extended_CompaniesDoesNotExist_DoesNotCallAnualService()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(new List<ElasticCompanyModelDTO>()));

            // Act
            var actual = await this.companyService.FindCompaniesAsync(companyIds, RequestType.Extended);

            // Assert
            Assert.Empty(actual);
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task SearchCompaniesAsync_RequestType_Light_Returns_FoundCustomers()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();
            string query = fixture.Create<string>();

            var companies = CreateElastincCompanyModels(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.SearchCompaniesAsync(query))
                .Returns(Task.FromResult(companies));

            // Act
            var actual = await this.companyService.SearchCompaniesAsync(query, RequestType.Ligth);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            this.companySearchServiceMock.Verify(a => a.SearchCompaniesAsync(query), Times.Once);
        }

        [Fact]
        public async Task SearchCompaniesAsync_RequestType_Extended_Returns_FoundCustomers()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();
            string query = fixture.Create<string>();

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.SearchCompaniesAsync(query))
                .Returns(Task.FromResult(companies));

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            // Act
            var actual = await this.companyService.SearchCompaniesAsync(query, RequestType.Extended);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            Assert.True(actual.All(r => r.AnnualReportAvailable));
            this.companySearchServiceMock.Verify(a => a.SearchCompaniesAsync(query), Times.Once);
        }

        [Fact]
        public async Task SearchCompaniesAsync_RequestType_Extended_AunalDoesNotExist_Returns_FoundCustomersWOAnualReposts()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();
            string query = fixture.Create<string>();

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, fixture.CreateMany<string>(3).ToList());

            this.companySearchServiceMock.Setup(s => s.SearchCompaniesAsync(query))
                .Returns(Task.FromResult(companies));

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            // Act
            var actual = await this.companyService.SearchCompaniesAsync(query, RequestType.Extended);

            // Assert
            Assert.Equal(companyIds, actual.Select(c => c.VAT));
            Assert.False(actual.All(r => r.AnnualReportAvailable));
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Once);
            this.companySearchServiceMock.Verify(a => a.SearchCompaniesAsync(query), Times.Once);
        }

        [Fact]
        public async Task SearchCompaniesAsync_RequestType_Extended_CompaniesDoesNotExist_DoesNotCallAnualService()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string query = fixture.Create<string>();

            this.companySearchServiceMock.Setup(s => s.SearchCompaniesAsync(query))
                .Returns(Task.FromResult(new List<ElasticCompanyModelDTO>()));

            // Act
            var actual = await this.companyService.SearchCompaniesAsync(query, RequestType.Extended);

            // Assert
            Assert.Empty(actual);
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task TypeaheadAsync_Returns_FoundCustomers()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List<string> companyIds = fixture.CreateMany<string>(3).ToList();
            string query = fixture.Create<string>();

            var companies = CreateElastincCompanyTypeaheadModels(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.SearchCompaniesTypeaheadAsync(query))
                .Returns(Task.FromResult(companies));

            // Act
            var actual = await this.companyService.TypeaheadAsync(query);

            // Assert
            for (int i = 0; i < companies.Count(); i++)
            {
                Assert.Equal(i + 1, actual[i].Id);
                Assert.Equal(companyIds[i], actual[i].RegistrationName);
                Assert.Equal("company_name (" + companyIds[i] + ")", actual[i].Label);
            }

            this.companySearchServiceMock.Verify(a => a.SearchCompaniesTypeaheadAsync(query), Times.Once);
        }

        [Fact]
        public async Task GetCompanyCreditDataByIdAsync_Returns_CustomerCretitData()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string companyId = fixture.Create<string>();
            List<string> companyIds = new List<string> { companyId };

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            Rating rating = fixture.Create<Rating>();
            this.scoreServiceMock.Setup(s => s.GetCompanyRating(It.IsAny<Company>()))
                .Returns(rating);

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            string generalRecomendation = fixture.Create<string>(); ;
            this.scoreServiceMock.Setup(s => s.MakeGeneralRecommendation(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(generalRecomendation);

            string deptCollectionRecomendation = fixture.Create<string>();
            this.scoreServiceMock.Setup(s => s.MakeDebtCollectionRecommendation(It.IsAny<int>(), It.IsAny<CompanyType>(), It.IsAny<bool>()))
                .Returns(deptCollectionRecomendation);

            // Act
            var actual = await this.companyService.GetCompanyCreditDataByIdAsync(companyId);

            // Assert
            Assert.Equal(companyId, actual.Registration);
            Assert.Equal(rating, actual.RatingData);
            Assert.Equal(generalRecomendation, actual.Recommendation);
            Assert.Equal(deptCollectionRecomendation, actual.DebtCollectionRecommendation);

            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async Task FindCompaniesCreditDataAsync_Returns_CustomersCretitData()
        {
            // Arrange
            Fixture fixture = new Fixture();
            List <string> companyIds = fixture.CreateMany<string>(3).ToList();

            var companies = CreateElastincCompanyModels(fixture, companyIds);
            var anualReports = CreateAnnualReports(fixture, companyIds);

            this.companySearchServiceMock.Setup(s => s.FindCompaniesAsync(companyIds))
                .Returns(Task.FromResult(companies));

            Rating rating = fixture.Create<Rating>();
            this.scoreServiceMock.Setup(s => s.GetCompanyRating(It.IsAny<Company>()))
                .Returns(rating);

            this.annualReportServiceMock.Setup(s => s.GetAnnualReports(companyIds))
                .Returns(Task.FromResult(anualReports));

            string generalRecomendation = fixture.Create<string>(); ;
            this.scoreServiceMock.Setup(s => s.MakeGeneralRecommendation(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(generalRecomendation);

            string deptCollectionRecomendation = fixture.Create<string>();
            this.scoreServiceMock.Setup(s => s.MakeDebtCollectionRecommendation(It.IsAny<int>(), It.IsAny<CompanyType>(), It.IsAny<bool>()))
                .Returns(deptCollectionRecomendation);

            // Act
            List<CreditData> actual = await this.companyService.FindCompaniesCreditDataAsync(companyIds);

            // Assert
            for (int i = 0; i < companies.Count(); i++)
            {
                Assert.Equal(companyIds[i], actual[i].Registration);
                Assert.Equal(rating, actual[i].RatingData);
                Assert.Equal(generalRecomendation, actual[i].Recommendation);
                Assert.Equal(deptCollectionRecomendation, actual[i].DebtCollectionRecommendation);
            }

            this.companySearchServiceMock.Verify(a => a.FindCompaniesAsync(companyIds), Times.Once);
            this.annualReportServiceMock.Verify(a => a.GetAnnualReports(It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        private List<ElasticCompanyModelDTO> CreateElastincCompanyModels(Fixture fixture, List<string> companiesSrv)
        {
            Nyestehovedbranche nyesteBibranche = fixture
                .Build<Nyestehovedbranche>()
                .With(t => t.branchekode, "12")
                .Create();

            Virksomhedmetadata virksomhedMetadata = fixture
                .Build<Virksomhedmetadata>()
                .With(t => t.nyesteHovedbranche, nyesteBibranche)
                .With(t => t.nyesteBibranche1, nyesteBibranche)
                .Create();

            List<Vrvirksomhed> vrvirksomheds = new List<Vrvirksomhed>();
            foreach (var srv in companiesSrv)
            {
                vrvirksomheds.Add(fixture
                .Build<Vrvirksomhed>()
                .With(t => t.virksomhedMetadata, virksomhedMetadata)
                .With(t => t.cvrNummer, srv)
                .Create());
            }

            List<ElasticCompanyModelDTO> companies = new List<ElasticCompanyModelDTO>();
            foreach (var vrvirksomhed in vrvirksomheds)
            {
                companies.Add(fixture
                    .Build<ElasticCompanyModelDTO>()
                    .With(t => t.Vrvirksomhed, vrvirksomhed)
                    .Create());
            }

            return companies;
        }

        private List<AnnualReportXMLData> CreateAnnualReports(Fixture fixture, List<string> companiesSrv)
        {
            List<AnnualReportXMLData> reports = new List<AnnualReportXMLData>();
            foreach(var srv in companiesSrv)
            {
                reports.Add(fixture
                    .Build<AnnualReportXMLData>()
                    .With(c => c.RegistrationNumber, srv)
                    .Create());
            }

            return reports;
        }

        private List<ElasticCompanyTypeaheadModelDTO> CreateElastincCompanyTypeaheadModels(Fixture fixture, List<string> companiesSrv)
        {
            NyestenavnTypeahead nyesteBibranche = fixture
                .Build<NyestenavnTypeahead>()
                .With(t => t.navn, "company_name")
                .Create();

            VirksomhedmetadataTypeahead virksomhedMetadata = fixture
                .Build<VirksomhedmetadataTypeahead>()
                .With(t => t.nyesteNavn, nyesteBibranche)
                .Create();

            List<VrvirksomhedTypeahead> vrvirksomheds = new List<VrvirksomhedTypeahead>();
            foreach (var srv in companiesSrv)
            {
                vrvirksomheds.Add(fixture
                .Build<VrvirksomhedTypeahead>()
                .With(t => t.virksomhedMetadata, virksomhedMetadata)
                .With(t => t.cvrNummer, srv)
                .Create());
            }

            List<ElasticCompanyTypeaheadModelDTO> companies = new List<ElasticCompanyTypeaheadModelDTO>();
            foreach (var vrvirksomhed in vrvirksomheds)
            {
                companies.Add(fixture
                    .Build<ElasticCompanyTypeaheadModelDTO>()
                    .With(t => t.Vrvirksomhed, vrvirksomhed)
                    .Create());
            }

            return companies;
        }
    }
}