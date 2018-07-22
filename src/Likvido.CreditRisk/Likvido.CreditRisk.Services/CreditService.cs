using System.Collections.Generic;
using Likvido.CreditRisk.Domain.Models.Credit;
using Likvido.CreditRisk.Services.Abstraction;
using Likvido.CreditRisk.DataAccess.Abstraction;
using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using AutoMapper;
using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;
using Likvido.CreditRisk.Domain.Exceptions;
using Likvido.CreditRisk.Domain.Entities.Registration;
using System.Linq.Expressions;
using System;

namespace Likvido.CreditRisk.Services
{
    public class CreditService : ICreditService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IMapper mapper;

        private readonly IScoreService scoreService;

        public CreditService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IMapper mapper,
            IScoreService scoreService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mapper = mapper;
            this.scoreService = scoreService;
        }

        public async Task<CreditPrivateData> GetCreditPrivateAsync(string registrationNumber)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationUserRepository>();
                var user = (await repo.FindAsync(x => x.RegistrationNumber == registrationNumber,
                    includes: new List<Expression<Func<RegistrationUser, object>>> { x => x.Registrations }))
                    .FirstOrDefault();
                if (user == null)
                {
                    throw new ResourceNotFoundException($"Private data with registration number {registrationNumber} was not found.");
                }

                var registrationUser = this.mapper.Map<RegistrationUser, RegistrationUserDTO>(user);

                return GetPrivateCreditData(registrationUser);
            }
        }

        public async Task<List<CreditPrivateData>> GetCreditPrivatesAsync(List<string> registrationNumbers)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationUserRepository>();
                var users = await repo.FindAsync(x => registrationNumbers.Contains(x.RegistrationNumber),
                    includes: new List<Expression<Func<RegistrationUser, object>>> { x => x.Registrations });

                var registrationUsers = this.mapper.Map<List<RegistrationUserDTO>>(users);

                return registrationUsers.Select(this.GetPrivateCreditData).ToList();
            }
        }

        public async Task<List<CreditPrivateData>> GetCreditPrivatesSearchAsync(string query)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<IRegistrationUserRepository>();
                var users = await repo.SearchAsync(query);                   

                var registrationUsers = this.mapper.Map<List<RegistrationUserDTO>>(users);

                return registrationUsers.Select(this.GetPrivateCreditData).ToList();
            }
        }

        private CreditPrivateData GetPrivateCreditData(RegistrationUserDTO registrationUser)
        {
            var privateSummaryRating = this.scoreService.CalculatePrivateSummaryRating(registrationUser.NumberOfRegistratins);
            var privateData = this.mapper.Map<PrivateData>(registrationUser);
            var result = new CreditPrivateData
            {
                Registration = registrationUser.RegistrationNumber,
                RatingData = new Rating { RegistrationNumber = registrationUser.RegistrationNumber, SummaryRating = privateSummaryRating },
                PrivateData = privateData,
                Recommendation = this.scoreService.MakeGeneralRecommendation(privateSummaryRating),
                DebtCollectionRecommendation = this.scoreService.MakeDebtCollectionRecommendation(privateSummaryRating, CompanyType.Personal)
            };

            return result;
        }
    }
}
