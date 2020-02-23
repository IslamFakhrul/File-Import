using FileImport.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileImport.Persistence.Database.Configurations
{
    public class ColorCodeConfiguration : IEntityTypeConfiguration<ColorCode>
    {
        public void Configure(EntityTypeBuilder<ColorCode> builder)
        {
            builder.ToTable("ColorCode");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}