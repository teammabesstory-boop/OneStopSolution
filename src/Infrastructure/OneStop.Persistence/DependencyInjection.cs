using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // <--- Pastikan ini ada
using Microsoft.Extensions.DependencyInjection;
using System; // Tambahan safety

namespace OneStop.Persistence
{
    // WAJIB: public static
    public static class DependencyInjection
    {
        // WAJIB: public static
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Safety check
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}