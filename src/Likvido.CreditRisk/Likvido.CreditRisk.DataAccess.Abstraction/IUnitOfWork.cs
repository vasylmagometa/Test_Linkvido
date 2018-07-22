using Likvido.CreditRisk.DataAccess.Abstraction.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.DataAccess.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        T GetRepository<T>() 
            where T : class, IBaseRepository;
    }
}
