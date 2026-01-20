using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OneStop.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // PENTING: Copy Connection String Anda di sini.
            // Factory ini HANYA jalan saat 'dotnet ef', jadi hardcode di sini aman untuk development.
            // Pastikan string ini SAMA PERSIS dengan yang ada di appsettings.json Anda.
            var connectionString = "Host=localhost;Port=5432;Database=OneStopDb;Username=postgres;Password=admin";

            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}