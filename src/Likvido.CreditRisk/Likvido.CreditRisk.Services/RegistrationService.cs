using System.Threading.Tasks;
using Likvido.CreditRisk.DataAccess.Abstraction;
using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.Domain.Entities.Registration;
using System;
using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using AutoMapper;
using Likvido.CreditRisk.Domain.Exceptions;
using Likvido.CreditRisk.Domain.Models.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Likvido.CreditRisk.Utils.DateTimeUtils;

namespace Likvido.CreditRisk.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IMapper mapper;

        public RegistrationService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mapper = mapper;
        }

        public async Task<Guid> CreateRegistrationPrivateAsync(RegistrationPrivateDTO createRegistration)
        {
            var registration = this.mapper.Map<Registration>(createRegistration);
            registration.DateCreated = TimeProvider.Current.UtcNow;
            RegistrationDataFilter userFilter = CreateRegistrationUserFilter(createRegistration);

            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();
                var registrationUserRepo = uow.GetRepository<IRegistrationUserRepository>();

                registration.PrivateData = await registrationUserRepo.FindRegistrationUser(userFilter)
                    ?? this.mapper.Map<RegistrationUser>(createRegistration);

                repo.Add(registration);

                await uow.SaveChangesAsync();

                return registration.Id;
            }
        }

        public async Task<Guid> CreateRegistrationCompanyAsync(RegistrationCompanyDTO createRegistration)
        {
            var registration = this.mapper.Map<Registration>(createRegistration);
            registration.DateCreated = TimeProvider.Current.UtcNow;
            RegistrationDataFilter companyFilter = CreateRegistrationCompanyFilter(createRegistration);

            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();
                var registrationCompanyRepo = uow.GetRepository<IRegistrationCompanyRepository>();

                registration.CompanyData = await registrationCompanyRepo.FindRegistrationCompany(companyFilter)
                    ?? this.mapper.Map<RegistrationCompany>(createRegistration);

                repo.Add(registration);

                await uow.SaveChangesAsync();

                return registration.Id;
            }
        }

        public async Task<RegistrationDetailsDTO> GetRegistrationByIdAsync(Guid id)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();
                var registration = await this.GetRegistrationById(repo, id);

                return this.Map(registration);
            }
        }

        public async Task<List<RegistrationDetailsDTO>> GetRegistrationsBySpecificationAsync(RegistrationSpecification specification)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();

                var registrations = await repo.GetRegistrationsBySpecificationAsync(specification);

                return registrations.Select(this.Map).ToList();
            }
        }

        public async Task<List<RegistrationDetailsDTO>> GetRegistrationsSearchAsync(string search)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();

                var registrations = await repo.GetRegistrationsSearchAsync(search);

                return registrations.Select(this.Map).ToList();
            }
        }

        public async Task DeleteRegistrationByIdAsync(Guid id)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationRepository>();
                var registration = await this.GetRegistrationById(repo, id);

                registration.DateDeleted = DateTime.UtcNow;
                repo.Update(registration);

                await uow.SaveChangesAsync();
            }
        }

        public async Task UpdatePrivateRegistrationAsync(Guid id, RegistrationPrivateDTO registration)
        {
            RegistrationDataFilter userFilter = CreateRegistrationUserFilter(registration);

            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                //Update registration
                var repo = uow.GetRepository<IRegistrationRepository>();
                var existingRegistration = await this.GetRegistrationById(repo, id);
                this.mapper.Map(registration, existingRegistration);
                existingRegistration.DateUpdated = DateTime.UtcNow;

                //Update private data
                var userRepo = uow.GetRepository<IRegistrationUserRepository>();
                var user = await userRepo.FindRegistrationUser(userFilter);
                if (user == null)
                {
                    user = new RegistrationUser();
                    userRepo.Add(user);

                }

                existingRegistration.PrivateData = user;
                this.mapper.Map(registration, user);

                await uow.SaveChangesAsync();
            }
        }

        public async Task UpdateCompanyRegistrationAsync(Guid id, RegistrationCompanyDTO registration)
        {
            RegistrationDataFilter companyFilter = CreateRegistrationCompanyFilter(registration);

            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                //Update registration
                var repo = uow.GetRepository<IRegistrationRepository>();
                var existingRegistration = await this.GetRegistrationById(repo, id);
                this.mapper.Map(registration, existingRegistration);
                existingRegistration.DateUpdated = DateTime.UtcNow;

                //Update company data
                var companyRepo = uow.GetRepository<IRegistrationCompanyRepository>();
                var company = await companyRepo.FindRegistrationCompany(companyFilter);
                if (company == null)
                {
                    company = new RegistrationCompany();
                    companyRepo.Add(company);

                }

                existingRegistration.CompanyData = company;
                this.mapper.Map(registration, company);

                await uow.SaveChangesAsync();
            }
        }

        private async Task<Registration> GetRegistrationById(IRegistrationRepository repository, Guid id)
        {
            var registration = (await repository.FindAsync(x => x.Id == id,
                includes: new List<Expression<Func<Registration, object>>> { x => x.PrivateData, x => x.CompanyData }))
                .FirstOrDefault();

            if (registration == null)
            {
                throw new ResourceNotFoundException($"Registration with id {id} was not found.");
            }

            return registration;
        }

        private RegistrationDetailsDTO Map(Registration registration)
        {
            RegistrationDetailsDTO registrationDetails = new RegistrationDetailsDTO();
            if (registration.PrivateData != null)
            {
                registrationDetails = this.mapper.Map<RegistrationPrivateDetailsDTO>(registration.PrivateData);
            }
            else if (registration.CompanyData != null)
            {
                registrationDetails = this.mapper.Map<RegistrationCompanyDetailsDTO>(registration.CompanyData);
            }

            return this.mapper.Map(registration, registrationDetails);
        }

        private static RegistrationDataFilter CreateRegistrationUserFilter(RegistrationPrivateDTO registration)
        {
            return new RegistrationDataFilter
            {
                Email = registration.Email,
                Phone = registration.Phone,
                RegistrationId = registration.RegistrationNumber
            };
        }

        private static RegistrationDataFilter CreateRegistrationCompanyFilter(RegistrationCompanyDTO registration)
        {
            return new RegistrationDataFilter
            {
                Email = registration.Email,
                Phone = registration.Phone,
                RegistrationId = registration.RegistrationName
            };
        }
    }
}
