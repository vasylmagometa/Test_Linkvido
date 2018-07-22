using Likvido.CreditRisk.Domain.DTOs;
using Likvido.CreditRisk.Domain.Models.Registration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Services.Abstraction
{
    public interface IRegistrationService
    {
        Task<Guid> CreateRegistrationPrivateAsync(RegistrationPrivateDTO createRegistration);

        Task<Guid> CreateRegistrationCompanyAsync(RegistrationCompanyDTO createRegistration);

        Task<RegistrationDetailsDTO> GetRegistrationByIdAsync(Guid id);

        Task<List<RegistrationDetailsDTO>> GetRegistrationsBySpecificationAsync(RegistrationSpecification specification);        

        Task<List<RegistrationDetailsDTO>> GetRegistrationsSearchAsync(string search);

        Task DeleteRegistrationByIdAsync(Guid id);

        Task UpdatePrivateRegistrationAsync(Guid id, RegistrationPrivateDTO registration);

        Task UpdateCompanyRegistrationAsync(Guid id, RegistrationCompanyDTO registration);
    }
}
