using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using Likvido.CreditRisk.DataAccess.Extensions;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Likvido.CreditRisk.Domain.Models.Registration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Repository
{
    public class RegistrationUserRepository : EntityFrameworkRepository<RegistrationUser, Guid>, IRegistrationUserRepository
    {
        public Task<RegistrationUser> FindRegistrationUser(RegistrationDataFilter filter)
        {
            return this.Queryable
                .FirstOrDefaultAsync(u => u.RegistrationNumber.Equals(filter.RegistrationId)
                    && u.Email.Equals(filter.Email)
                    && u.Phone.Equals(filter.Phone));
        }

        public Task<List<RegistrationUser>> SearchAsync(string query)
        {           
            return 
                this.Queryable
                .Include(x => x.Registrations)
                .Search(query)
                .ToListAsync();
        }       
    }
}
