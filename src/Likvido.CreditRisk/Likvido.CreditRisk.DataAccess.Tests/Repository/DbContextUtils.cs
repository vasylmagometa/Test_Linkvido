using Likvido.CreditRisk.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Likvido.CreditRisk.DataAccess.Tests.Repository
{
    public class DbContextUtils
    {
        public static LikvidoDbContext CreateInMemoryDbContext([CallerMemberName] string databaseName = "Test")
        {
            var options = new DbContextOptionsBuilder<LikvidoDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new LikvidoDbContext(options);
        }
    }
}
