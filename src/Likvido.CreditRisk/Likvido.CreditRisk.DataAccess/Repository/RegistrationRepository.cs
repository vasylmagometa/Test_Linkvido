using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using Likvido.CreditRisk.Domain.Entities.Registration;
using System;
using Likvido.CreditRisk.Domain.Models.Registration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Likvido.CreditRisk.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Likvido.CreditRisk.DataAccess.Repository
{
    public class RegistrationRepository : EntityFrameworkRepository<Registration, Guid>, IRegistrationRepository
    {
        public RegistrationRepository() { }

        public RegistrationRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Registration>> GetRegistrationsBySpecificationAsync(RegistrationSpecification specification)
        {
            return this.Queryable          
                .Include(x => x.CompanyData)
                .Include(x => x.PrivateData)
                .WhereOr(specification.Ids, x => y => y.Id.ToString() == x)
                .WhereOr(specification.InvoiceIds, x => y => y.InvoiceId.ToString() == x)
                .WhereOr(specification.DebtorIds, x => y => y.DeptorId.ToString() == x)  
                .Where(x => !x.DateDeleted.HasValue)                
                .ToListAsync();
        }

        public Task<List<Registration>> GetRegistrationsSearchAsync(string search)
        {
            return this.Queryable
                .Include(x => x.CompanyData)
                .Include(x => x.PrivateData)
                .Where(x => !x.DateDeleted.HasValue)                
                .Search(search)
                .ToListAsync();
        }
    }
}
