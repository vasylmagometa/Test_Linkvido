using Likvido.CreditRisk.Domain.Entities.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Likvido.CreditRisk.DataAccess.Mapping
{
    public class RegistrationCompanyMap : IEntityTypeConfiguration<RegistrationCompany>
    {
        public void Configure(EntityTypeBuilder<RegistrationCompany> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("RegistrationCompanies");
        }
    }
}
