using Microsoft.EntityFrameworkCore;

namespace Likvido.CreditRisk.DataAccess.Abstraction.Repository
{
    public interface IBaseRepository
    {
        void SetContext(DbContext context);
    }
}
