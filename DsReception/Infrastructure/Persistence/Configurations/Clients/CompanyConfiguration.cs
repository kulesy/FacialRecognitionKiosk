using DsReceptionClassLibrary.Domain.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DsReceptionAPI.Infrastructure.Persistence.Configurations.Clients
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(m => m.CompanyId);
            
            builder.Property(m => m.Name)
               .HasMaxLength(50);

            builder.Property(m => m.Address)
              .HasMaxLength(100);

            builder.Property(m => m.Phone)
              .HasMaxLength(20);
        }
    }
}
