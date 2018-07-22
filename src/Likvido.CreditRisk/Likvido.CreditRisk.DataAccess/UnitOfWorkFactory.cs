using Likvido.CreditRisk.DataAccess.Abstraction;
using Likvido.CreditRisk.DataAccess.DataContext;
using Likvido.CreditRisk.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace Likvido.CreditRisk.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConfigurationManager configurationManager;

        private readonly IServiceProvider serviceProvider;

        public UnitOfWorkFactory(
            IConfigurationManager configurationManager,
            IServiceProvider serviceProvider)
        {
            this.configurationManager = configurationManager;
            this.serviceProvider = serviceProvider;
        }
        public IUnitOfWork CreateUnitOfWork()
        {
            var dbContext = this.CreateDbContext();
            var unitOfWork = new UnitOfWork(dbContext, this.serviceProvider);

            return unitOfWork;
        }

        private DbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LikvidoDbContext>()
                .UseSqlServer(this.configurationManager.LikvidoDatabaseConnectionString)
                .Options;
            DbContext dbContext = new LikvidoDbContext(dbContextOptions);

            return dbContext;
        }
    }
}
