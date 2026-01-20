using System;
using System.Collections.Generic;
using OneStop.Domain.Common.Primitives;

namespace OneStop.Domain.Modules.Production
{
    public enum RecipeItemType
    {
        RawMaterial,
        SubRecipe
    }

    public class RecipeItem : ValueObject
    {
        // 1. Constructor Kosong untuk EF Core (WAJIB ADA)
#pragma warning disable CS8618
        private RecipeItem() { }
#pragma warning restore CS8618

        // Constructor Asli untuk Logika Aplikasi
        internal RecipeItem(Guid componentId, RecipeItemType type, decimal quantity, Unit unit)
        {
            ComponentId = componentId;
            Type = type;
            Quantity = quantity;
            Unit = unit;
        }

        // 2. Tambahkan 'private set' agar EF Core bisa mengisi data ke property ini
        public Guid ComponentId { get; private set; }
        public RecipeItemType Type { get; private set; }
        public decimal Quantity { get; private set; }
        public Unit Unit { get; private set; }

        public static RecipeItem CreateIngredient(Guid ingredientId, decimal qty, Unit unit)
        {
            return new RecipeItem(ingredientId, RecipeItemType.RawMaterial, qty, unit);
        }

        public static RecipeItem CreateSubRecipe(Guid subRecipeId, decimal qty, Unit unit)
        {
            return new RecipeItem(subRecipeId, RecipeItemType.SubRecipe, qty, unit);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return ComponentId;
            yield return Type;
            yield return Quantity;
            yield return Unit;
        }
    }
}