using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Repository
{
    public class RegistrationCompanyRepository : EntityFrameworkRepository<RegistrationCompany, Guid>, IRegistrationCompanyRepository
    {
        public Task<RegistrationCompany> FindRegistrationCompany(RegistrationDataFilter filter)
        {
            return this.Queryable
                    .FirstOrDefaultAsync(u => u.RegistrationName.Equals(filter.RegistrationId)
                            && u.Email.Equals(filter.Email)
                            && u.Phone.Equals(filter.Phone));
        }
    }
}
