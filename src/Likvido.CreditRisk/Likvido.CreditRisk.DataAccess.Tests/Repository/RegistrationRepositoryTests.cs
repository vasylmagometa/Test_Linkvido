using AutoFixture;
using Likvido.CreditRisk.DataAccess.Repository;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System;
using System.Linq.Expressions;

namespace Likvido.CreditRisk.DataAccess.Tests.Repository
{
    public class RegistrationRepositoryTests
    {
        [Fact]
        public async Task GetRegistrationsBySpecificationAsync_ForSpecifiedIds_ReturnsRegistrationsByIds()
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Without(x => x.DateDeleted)
                .CreateMany(3)
                .ToList();

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var specification = new RegistrationSpecification();
            specification.Ids.AddRange(new[] { registrations[0].Id.ToString(), registrations[1].Id.ToString() });

            var repository = new RegistrationRepository(dbContext);

            // Act
            var result = await repository.GetRegistrationsBySpecificationAsync(specification);

            // Assert
            result.Should().BeEquivalentTo(registrations[0], registrations[1]);
        }

        [Fact]
        public async Task GetRegistrationsBySpecificationAsync_ForSpecifiedInvoiceIds_ReturnsRegistrationsByInvoiceIds()
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Without(x => x.DateDeleted)
                .CreateMany(3)
                .ToList();

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var specification = new RegistrationSpecification();
            specification.InvoiceIds.AddRange(new[] { registrations[0].InvoiceId.ToString(), registrations[1].InvoiceId.ToString() });

            var repository = new RegistrationRepository(dbContext);

            // Act
            var result = await repository.GetRegistrationsBySpecificationAsync(specification);

            // Assert
            result.Should().BeEquivalentTo(registrations[0], registrations[1]);
        }

        [Fact]
        public async Task GetRegistrationsBySpecificationAsync_ForSpecifiedDeptorIds_ReturnsRegistrationsByDeptorIds()
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Without(x => x.DateDeleted)
                .CreateMany(3)
                .ToList();

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var specification = new RegistrationSpecification();
            specification.DebtorIds.AddRange(new[] { registrations[0].DeptorId.ToString(), registrations[1].DeptorId.ToString() });

            var repository = new RegistrationRepository(dbContext);

            // Act
            var result = await repository.GetRegistrationsBySpecificationAsync(specification);

            // Assert
            result.Should().BeEquivalentTo(registrations[0], registrations[1]);
        }

        [Fact]
        public async Task GetRegistrationsBySpecificationAsync_ForSpecification_ExcludesDeletedRegistrations()
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .With(x => x.DateDeleted, DateTime.UtcNow)
                .CreateMany(3)
                .ToList();

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var specification = new RegistrationSpecification();
            specification.Ids.Add(registrations[0].Id.ToString());
            specification.Ids.Add(registrations[2].Id.ToString());

            var repository = new RegistrationRepository(dbContext);

            // Act
            var result = await repository.GetRegistrationsBySpecificationAsync(specification);

            // Assert
            result.Should().NotContain(registrations[2]);
        }

        [Fact]
        public async Task GetRegistrationsBySpecificationAsync_ForEmptySpecification_ReturnsAll()
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Without(x => x.DateDeleted)
                .CreateMany(3)
                .ToList();

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var specification = new RegistrationSpecification();

            var repository = new RegistrationRepository(dbContext);

            // Act
            var result = await repository.GetRegistrationsBySpecificationAsync(specification);

            // Assert
            result.Should().BeEquivalentTo(registrations);
        }

        [Theory]
        [MemberData(nameof(GetRegistrationSearchObjects))]
        public async Task GetRegistrationsSearchAsync_ForSpecifiedSearchQuery_ReturnsFoundRegistrations(string searchText, Registration matchingRegistration)
        {
            // Arrange
            var fixture = new Fixture();
            var registrations = fixture
                .Build<Registration>()
                .Without(x => x.PrivateData)
                .Without(x => x.CompanyData)
                .Without(x => x.DateDeleted)
                .CreateMany(3)
                .ToList();

            registrations.Add(matchingRegistration);

            var dbContext = DbContextUtils.CreateInMemoryDbContext();
            dbContext.Registrations.AddRange(registrations);
            await dbContext.SaveChangesAsync();

            var repository = new RegistrationRepository(dbContext);

            // Act
            List<Registration> result = await repository.GetRegistrationsSearchAsync(searchText);

            // Assert
            result.Select(x => x.Id).Should().BeEquivalentTo(matchingRegistration.Id);
        }

        public static IEnumerable<object[]> GetRegistrationSearchObjects()
        {
            var fixture = new Fixture();

            var registrationSearchableProperties = new Expression<Func<Registration, string>>[]
            {
                x => x.Description,
                x => x.Foundation,
                x => x.AdministratorName,
                x => x.AdministratorPhone,
                x => x.AdministratorReference,
                x => x.CreditorName,
                x => x.CreditorPhone,
                x => x.CreditorReference
            };

            var registrationUserSearchableProperties = new Expression<Func<RegistrationUser, string>>[]
            {
                x => x.Address
            };

            var registrationCompanySearchableProperties = new Expression<Func<RegistrationCompany, string>>[]
            {
                x => x.Address
            };

            // should search by Registration Properties
            List<object[]> searchObjects = new List<object[]>();
            foreach (var property in registrationSearchableProperties)
            {
                string searchQuery = Guid.NewGuid().ToString();
                var registration = fixture.Build<Registration>()
                    .With(property, GetStringWith(searchQuery))
                    .Without(x => x.PrivateData)
                    .Without(x => x.CompanyData)
                    .Without(x => x.DateDeleted)
                    .Create();
                searchObjects.Add(new object[] { searchQuery, registration });
            }

            // should search by Registration Private Data
            foreach (var property in registrationUserSearchableProperties)
            {
                string searchQuery = Guid.NewGuid().ToString();

                var registrationUser = fixture.Build<RegistrationUser>().With(property, GetStringWith(searchQuery)).Without(x => x.Registrations).Create();

                var registration = fixture.Build<Registration>()
                    .With(x => x.PrivateData, registrationUser)
                    .Without(x => x.CompanyData)
                    .Without(x => x.DateDeleted)
                    .Create();
                searchObjects.Add(new object[] { searchQuery, registration });
            }

            // should search by Registration Company Data
            foreach (var property in registrationCompanySearchableProperties)
            {
                string searchQuery = Guid.NewGuid().ToString();

                var registrationCompany = fixture.Build<RegistrationCompany>().With(property, GetStringWith(searchQuery)).Without(x => x.Registrations).Create();

                var registration = fixture.Build<Registration>()
                    .Without(x => x.PrivateData)
                    .With(x => x.CompanyData, registrationCompany)
                    .Without(x => x.DateDeleted)
                    .Create();
                searchObjects.Add(new object[] { searchQuery, registration });
            }

            return searchObjects;
        }

        private static string GetStringWith(string text)
        {
            return $"begin{text}end";
        }
    }
}
