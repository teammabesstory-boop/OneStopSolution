using System;
using System.Collections.Generic;
using OneStop.Domain.Common.Primitives;
using OneStop.Domain.Common.Shared; // <-- Update Namespace

namespace OneStop.Domain.Modules.Production
{
    public sealed class Recipe : Entity
    {
        private readonly List<RecipeItem> _items = new();

        private Recipe(Guid id, string name, decimal outputQuantity, Unit outputUnit)
            : base(id)
        {
            Name = name;
            OutputQuantity = outputQuantity;
            OutputUnit = outputUnit;
        }

#pragma warning disable CS8618
        private Recipe() { }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public decimal OutputQuantity { get; private set; }
        public Unit OutputUnit { get; private set; }
        public IReadOnlyCollection<RecipeItem> Items => _items.AsReadOnly();

        public decimal CachedCostPerUnit { get; private set; }
        public DateTime LastCostCalculation { get; private set; }

        public static Result<Recipe> Create(string name, decimal outputQty, Unit outputUnit)
        {
            if (outputQty <= 0)
                return Result.Failure<Recipe>(new Error("Recipe.InvalidOutput", "Output quantity must be > 0"));

            return new Recipe(Guid.NewGuid(), name, outputQty, outputUnit);
        }

        public Result AddIngredient(Ingredient ingredient, decimal quantity, Unit unit)
        {
            var item = RecipeItem.CreateIngredient(ingredient.Id, quantity, unit);
            _items.Add(item);
            return Result.Success();
        }

        public Result AddSubRecipe(Recipe subRecipe, decimal quantity, Unit unit)
        {
            if (subRecipe.Id == this.Id)
                return Result.Failure(new Error("Recipe.Circular", "Cannot add recipe to itself"));

            var item = RecipeItem.CreateSubRecipe(subRecipe.Id, quantity, unit);
            _items.Add(item);
            return Result.Success();
        }

        public void RecalculateCost(IDictionary<Guid, decimal> componentCosts)
        {
            decimal totalBatchCost = 0;
            foreach (var item in _items)
            {
                if (componentCosts.TryGetValue(item.ComponentId, out decimal costPerUnit))
                {
                    totalBatchCost += costPerUnit * item.Quantity;
                }
            }
            CachedCostPerUnit = totalBatchCost / OutputQuantity;
            LastCostCalculation = DateTime.UtcNow;
        }
    }
}