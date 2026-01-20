using Microsoft.EntityFrameworkCore;
using OneStop.Domain.Modules.Inventory;
using OneStop.Domain.Modules.Production;

namespace OneStop.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<StockMutation> StockMutations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MAGIC LINE: Scan semua file Configuration (IEntityTypeConfiguration) di project ini
            // dan terapkan mappingnya secara otomatis.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}