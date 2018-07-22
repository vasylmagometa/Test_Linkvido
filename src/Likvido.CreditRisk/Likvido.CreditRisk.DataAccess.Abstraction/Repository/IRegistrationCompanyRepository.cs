using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using System;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Abstraction.Repository
{
    public interface IRegistrationCompanyRepository : IRepository<RegistrationCompany, Guid>
    {
        Task<RegistrationCompany> FindRegistrationCompany(RegistrationDataFilter filter);
    }
}
