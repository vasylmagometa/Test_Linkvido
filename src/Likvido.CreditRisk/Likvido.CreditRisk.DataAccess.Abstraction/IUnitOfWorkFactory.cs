namespace Likvido.CreditRisk.DataAccess.Abstraction
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
