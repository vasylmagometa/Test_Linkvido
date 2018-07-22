using Likvido.CreditRisk.Domain.Entities.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Likvido.CreditRisk.DataAccess.Mapping
{
    public class RegistrationUserMap : IEntityTypeConfiguration<RegistrationUser>
    {
        public void Configure(EntityTypeBuilder<RegistrationUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("RegistrationUsers");            
        }
    }
}
