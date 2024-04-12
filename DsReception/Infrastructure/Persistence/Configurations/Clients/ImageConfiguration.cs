using DsReceptionClassLibrary.Domain.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DsReceptionAPI.Infrastructure.Persistence.Configurations.Clients
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(m => m.ImageId);

            builder.Property(m => m.FileName)
               .HasMaxLength(200);
        }
    }
}
