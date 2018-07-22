using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Abstraction.Repository
{
    public interface IRegistrationRepository : IRepository<Registration, Guid>
    {
        Task<List<Registration>> GetRegistrationsBySpecificationAsync(RegistrationSpecification specification);

        Task<List<Registration>> GetRegistrationsSearchAsync(string search);
    }
}
