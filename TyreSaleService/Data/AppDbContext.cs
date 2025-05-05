using Microsoft.EntityFrameworkCore;
using TyreSaleService.Models;

namespace TyreSaleService.Data
{
    /// <summary>
    /// Database context for the Tyre Sale Service.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
        public DbSet<TyreCompany> TyreCompanies { get; set; }
        public DbSet<TyreModel> TyreModels { get; set; }
        public DbSet<Tyre> Tyres { get; set; }

        /// <summary>
        /// Configures the model for the Tyre Sale Service.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TyreCompany>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Id)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Sqlite:Autoincrement", true);
            });

            // TyreModel
            builder.Entity<TyreModel>(b =>
            {
                b.HasKey(m => m.Id);
                b.Property(m => m.Id)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Sqlite:Autoincrement", true);

                b.HasOne(m => m.Company)
                 .WithMany(c => c.Models)
                 .HasForeignKey(m => m.CompanyId);
            });

            // Tyre
            builder.Entity<Tyre>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.Id)
                 .ValueGeneratedOnAdd()
                 .HasAnnotation("Sqlite:Autoincrement", true);

                b.OwnsOne(t => t.Dimensions);
                b.HasOne(t => t.Model)
                 .WithMany(m => m.Tyres)
                 .HasForeignKey(t => t.ModelId);
            });

            base.OnModelCreating(builder);
        }
    }
}