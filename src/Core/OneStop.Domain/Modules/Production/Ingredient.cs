using System;
using OneStop.Domain.Common.Primitives;
using OneStop.Domain.Common.Shared;

namespace OneStop.Domain.Modules.Production
{
    public sealed class Ingredient : Entity
    {
        // Constructor Private (untuk EF Core & Factory Method)
        private Ingredient(Guid id, string name, string sku, Unit unit, decimal currentPricePerUnit)
            : base(id)
        {
            Name = name;
            Sku = sku;
            Unit = unit; // Rename dari StockUnit ke Unit biar cocok sama XAML
            CurrentPricePerUnit = currentPricePerUnit;

            // Default Value
            CurrentStock = 0;
            MinimumStock = 10; // Default warning level
        }

        // Constructor kosong untuk EF Core
#pragma warning disable CS8618
        private Ingredient() { }
#pragma warning restore CS8618

        // PROPERTIES (Harus public agar bisa dibaca UI)
        public string Name { get; private set; }
        public string Sku { get; private set; }

        // Property ini yang dicari oleh XAML "Binding Unit.Name"
        public Unit Unit { get; private set; }

        // Property ini yang dicari oleh XAML "Binding CurrentStock" (ERROR YANG ANDA ALAMI)
        public decimal CurrentStock { get; private set; }

        public decimal MinimumStock { get; private set; }

        public decimal CurrentPricePerUnit { get; private set; }
        public DateTime LastPriceUpdate { get; private set; }

        // FACTORY METHOD
        public static Result<Ingredient> Create(string name, string sku, Unit unit, decimal initialPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Ingredient>(new Error("Ingredient.EmptyName", "Name is required"));

            if (initialPrice < 0)
                return Result.Failure<Ingredient>(new Error("Ingredient.NegativePrice", "Price cannot be negative"));

            return new Ingredient(Guid.NewGuid(), name, sku, unit, initialPrice);
        }

        // DOMAIN METHODS (Logic Bisnis)
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice != CurrentPricePerUnit)
            {
                CurrentPricePerUnit = newPrice;
                LastPriceUpdate = DateTime.UtcNow;
            }
        }

        // Tambahan method untuk mengubah stok (Logic sederhana dulu)
        public void AdjustStock(decimal amount)
        {
            CurrentStock += amount;
            // Validasi stok negatif bisa ditambahkan di sini
        }
    }
}