using System;
using OneStop.Domain.Common.Primitives;
using OneStop.Domain.Common.Shared; // <-- Update Namespace

namespace OneStop.Domain.Modules.Production
{
    public sealed class Ingredient : Entity
    {
        private Ingredient(Guid id, string name, string sku, Unit stockUnit, decimal currentPricePerUnit)
            : base(id)
        {
            Name = name;
            Sku = sku;
            StockUnit = stockUnit;
            CurrentPricePerUnit = currentPricePerUnit;
        }

        // Fix Warning CS8618 untuk EF Core
#pragma warning disable CS8618
        private Ingredient() { }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public string Sku { get; private set; }
        public Unit StockUnit { get; private set; }
        public decimal CurrentPricePerUnit { get; private set; }

        public DateTime LastPriceUpdate { get; private set; }

        public static Result<Ingredient> Create(string name, string sku, Unit unit, decimal initialPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Ingredient>(new Error("Ingredient.EmptyName", "Name is required"));

            if (initialPrice < 0)
                return Result.Failure<Ingredient>(new Error("Ingredient.NegativePrice", "Price cannot be negative"));

            return new Ingredient(Guid.NewGuid(), name, sku, unit, initialPrice);
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice != CurrentPricePerUnit)
            {
                CurrentPricePerUnit = newPrice;
                LastPriceUpdate = DateTime.UtcNow;
            }
        }
    }
}