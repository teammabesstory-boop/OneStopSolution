using Microsoft.EntityFrameworkCore;
using OneStop.Application.Common.Interfaces;
using OneStop.Domain.Modules.Production;
using OneStop.Persistence; // Pastikan referensi ke Persistence ada
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneStop.Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            // SELECT * FROM Ingredients
            return await _context.Ingredients
                .AsNoTracking() // Optimasi untuk read-only data
                .ToListAsync();
        }

        public async Task<Ingredient> AddIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }
    }
}