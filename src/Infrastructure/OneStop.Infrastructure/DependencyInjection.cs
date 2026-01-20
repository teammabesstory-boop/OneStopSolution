using Microsoft.Extensions.DependencyInjection;
using OneStop.Application.Common.Interfaces;
using OneStop.Infrastructure.Services;

namespace OneStop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Mendaftarkan InventoryService agar bisa di-inject via Interface IInventoryService
            // Menggunakan Lifetime 'Transient' (Service dibuat baru setiap kali diminta)
            services.AddTransient<IInventoryService, InventoryService>();

            return services;
        }
    }
}