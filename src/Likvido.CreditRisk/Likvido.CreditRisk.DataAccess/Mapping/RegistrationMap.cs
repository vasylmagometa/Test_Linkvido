using Likvido.CreditRisk.Domain.Entities.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Likvido.CreditRisk.DataAccess.Mapping
{
    public class RegistrationMap : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.PrivateData)
                .WithMany(x => x.Registrations)
                .HasForeignKey(x => x.PrivateDataId);

            builder.ToTable("Registrations");
        }
    }
}
