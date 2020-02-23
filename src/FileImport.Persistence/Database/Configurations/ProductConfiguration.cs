using FileImport.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileImport.Persistence.Database.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ArtikelCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DiscountPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DeliveredIn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Q1)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ColorCodeId)
                .IsRequired();

            builder.Property(x => x.ColorId)
                .IsRequired();

            builder.HasOne(p => p.Color)
                .WithMany(c => c.Products)
                .HasForeignKey(fk => fk.ColorId)
                .HasConstraintName("FK_Product_Color");

            builder.HasOne(p => p.ColorCode)
                .WithMany(cc => cc.Products)
                .HasForeignKey(fk => fk.ColorCodeId)
                .HasConstraintName("FK_Product_ColorCode");
        }
    }
}