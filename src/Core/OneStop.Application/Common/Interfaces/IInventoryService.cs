using OneStop.Domain.Modules.Production;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneStop.Application.Common.Interfaces
{
    public interface IInventoryService
    {
        // Kontrak: Ambil semua data & Simpan data baru
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> AddIngredientAsync(Ingredient ingredient);
    }
}