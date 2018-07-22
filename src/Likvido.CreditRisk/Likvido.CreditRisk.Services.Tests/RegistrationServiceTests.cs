using AutoFixture;
using AutoMapper;
using Likvido.CreditRisk.DataAccess.Abstraction;
using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using Likvido.CreditRisk.Utils.DateTimeUtils;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Likvido.CreditRisk.Services.Tests
{
    public class RegistrationServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkFake;

        private Mock<IRegistrationUserRepository> registrationUserRepositoryFake;

        private Mock<IRegistrationCompanyRepository> registrationCompanyRepositoryFake;

        private Mock<IRegistrationRepository> registrationRepositoryFake;

        private Mock<TimeProvider> timeProviderFake;

        private Mock<IMapper> mapperFake;

        private RegistrationService registrationService;

        public RegistrationServiceTests()
        {
            this.mapperFake = new Mock<IMapper>();

            this.timeProviderFake = new Mock<TimeProvider>();
            TimeProvider.Current = this.timeProviderFake.Object;

            this.SetupUnitOfWork();

            var unitOfWorkFactoryFake = new Mock<IUnitOfWorkFactory>();
            unitOfWorkFactoryFake.Setup(x => x.CreateUnitOfWork()).Returns(this.unitOfWorkFake.Object);
            this.registrationService = new RegistrationService(unitOfWorkFactoryFake.Object, this.mapperFake.Object);
        }

        [Fact]
        public async Task CreateRegistrationPrivateAsync_IfRegistrationUserAlreadyExists_LinksNewRegistrationToExistingUser()
        {
            // Arrange
            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationPrivateDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            var dummyRegistrationUser = fixture.Build<RegistrationUser>()
               .Without(x => x.Registrations)
               .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            this.SetupFindRegistrationUserBy(createRegistrationDto, dummyRegistrationUser);

            // Act
            await this.registrationService.CreateRegistrationPrivateAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.PrivateData == dummyRegistrationUser)));
        }

        [Fact]
        public async Task CreateRegistrationPrivateAsync_IfRegistrationUserDoesNotExist_CreatesRegistrationAndUser()
        {
            // Arrange
            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationPrivateDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            var dummyRegistrationUser = fixture.Build<RegistrationUser>()
               .Without(x => x.Registrations)
               .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            this.SetupMapping(createRegistrationDto, dummyRegistrationUser);

            this.SetupFindRegistrationUserBy(createRegistrationDto, null);

            // Act
            await this.registrationService.CreateRegistrationPrivateAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.PrivateData == dummyRegistrationUser)));
        }

        [Fact]
        public async Task CreateRegistrationPrivateAsync_Always_SetsUtcNowDateCreated()
        {
            // Arrange
            var fakeNowDate = new DateTime(2018, 7, 16);
            this.SetupUtcNow(fakeNowDate);

            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationPrivateDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            // Act
            await this.registrationService.CreateRegistrationPrivateAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.DateCreated == fakeNowDate)));
        }

        [Fact]
        public async Task CreateRegistrationCompanyAsync_IfRegistrationCompanyAlreadyExists_LinksNewRegistrationToExistingUser()
        {
            // Arrange
            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationCompanyDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            var dummyRegistrationCompany = fixture.Build<RegistrationCompany>()
               .Without(x => x.Registrations)
               .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            this.SetupFindRegistrationCompanyBy(createRegistrationDto, dummyRegistrationCompany);

            // Act
            await this.registrationService.CreateRegistrationCompanyAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.CompanyData == dummyRegistrationCompany)));
        }

        [Fact]
        public async Task CreateRegistrationCompanyAsync_IfRegistrationCompanyDoesNotExist_CreatesRegistrationAndUser()
        {
            // Arrange
            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationCompanyDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            var dummyRegistrationCompany = fixture.Build<RegistrationCompany>()
               .Without(x => x.Registrations)
               .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            this.SetupMapping(createRegistrationDto, dummyRegistrationCompany);

            this.SetupFindRegistrationCompanyBy(createRegistrationDto, null);

            // Act
            await this.registrationService.CreateRegistrationCompanyAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.CompanyData == dummyRegistrationCompany)));
        }

        [Fact]
        public async Task CreateRegistrationCompanyAsync_Always_SetsUtcNowDateCreated()
        {
            // Arrange
            var fakeNowDate = new DateTime(2018, 7, 16);
            this.SetupUtcNow(fakeNowDate);

            var fixture = new Fixture();
            var createRegistrationDto = fixture.Create<RegistrationCompanyDTO>();
            var dummyRegistration = fixture.Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Create();

            this.SetupMapping(createRegistrationDto, dummyRegistration);

            // Act
            await this.registrationService.CreateRegistrationCompanyAsync(createRegistrationDto);

            // Assert
            this.registrationRepositoryFake.Verify(x => x.Add(It.Is<Registration>(y => y.DateCreated == fakeNowDate)));
        }

        private void SetupUnitOfWork()
        {
            this.unitOfWorkFake = new Mock<IUnitOfWork>();

            this.registrationRepositoryFake = this.SetupRepository<IRegistrationRepository>();

            this.registrationCompanyRepositoryFake = this.SetupRepository<IRegistrationCompanyRepository>();

            this.registrationUserRepositoryFake = this.SetupRepository<IRegistrationUserRepository>();

            this.unitOfWorkFake.Setup(x => x.Dispose());
        }

        private Mock<T> SetupRepository<T>()
            where T : class, IBaseRepository
        {
            var repositoryFake = new Mock<T>();

            this.unitOfWorkFake.Setup(x => x.GetRepository<T>()).Returns(repositoryFake.Object);

            return repositoryFake;
        }

        private void SetupUtcNow(DateTime fakeNowDate)
        {
            this.timeProviderFake.SetupGet(tp => tp.UtcNow).Returns(fakeNowDate);
        }

        private void SetupMapping<TFrom, TTo>(TFrom fromObject, TTo toObject)
        {
            this.mapperFake.Setup(x => x.Map<TTo>(fromObject)).Returns(toObject);
        }

        private void SetupFindRegistrationUserBy(RegistrationPrivateDTO filter, RegistrationUser dummyRegistrationUser)
        {
            this.registrationUserRepositoryFake
                .Setup(x => x.FindRegistrationUser(It.Is<RegistrationDataFilter>(y => y.Email == filter.Email && y.Phone == filter.Phone && y.RegistrationId == filter.RegistrationNumber)))
                .ReturnsAsync(dummyRegistrationUser);
        }

        private void SetupFindRegistrationCompanyBy(RegistrationCompanyDTO filter, RegistrationCompany dummyRegistrationCompany)
        {
            this.registrationCompanyRepositoryFake
                .Setup(x => x.FindRegistrationCompany(It.Is<RegistrationDataFilter>(y => y.Email == filter.Email && y.Phone == filter.Phone && y.RegistrationId == filter.RegistrationName)))
                .ReturnsAsync(dummyRegistrationCompany);
        }
    }
}
