using FileImport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileImport.Persistence.Database
{
    public class FileImportDbContext : DbContext
    {
        public FileImportDbContext() : base()
        {
        }

        public FileImportDbContext(DbContextOptions<FileImportDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ColorCode> ColorCodes { get; set; }

        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileImportDbContext).Assembly);
        }
    }
}
