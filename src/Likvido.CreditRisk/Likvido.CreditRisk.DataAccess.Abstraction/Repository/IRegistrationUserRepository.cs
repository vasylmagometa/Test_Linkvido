using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Abstraction.Repository
{
    public interface IRegistrationUserRepository : IRepository<RegistrationUser, Guid>
    {
        Task<List<RegistrationUser>> SearchAsync(string query);

        Task<RegistrationUser> FindRegistrationUser(RegistrationDataFilter filter);
    }
}
