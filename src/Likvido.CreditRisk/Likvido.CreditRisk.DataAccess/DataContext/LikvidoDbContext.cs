using Likvido.CreditRisk.DataAccess.Mapping;
using Likvido.CreditRisk.Domain.Entities.Registration;
using Microsoft.EntityFrameworkCore;

namespace Likvido.CreditRisk.DataAccess.DataContext
{
    public class LikvidoDbContext : DbContext
    {
        public LikvidoDbContext(DbContextOptions<LikvidoDbContext> options)
            :base(options)
        { }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<RegistrationCompany> RegistrationCompanies { get; set; }

        public DbSet<RegistrationUser> RegistrationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RegistrationMap());
            modelBuilder.ApplyConfiguration(new RegistrationUserMap());
            modelBuilder.ApplyConfiguration(new RegistrationCompanyMap());            
        }
    }
}
