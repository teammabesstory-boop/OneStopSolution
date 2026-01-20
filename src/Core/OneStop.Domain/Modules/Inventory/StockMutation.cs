using System;
using OneStop.Domain.Common.Primitives;
using OneStop.Domain.Modules.Production;

namespace OneStop.Domain.Modules.Inventory
{
    public enum MutationType
    {
        PurchaseIn, ProductionOut, ProductionIn, Waste, Adjustment
    }

    public class StockMutation : Entity
    {
        internal StockMutation(Guid id, Guid itemId, MutationType type, decimal quantity, Unit unit, string referenceDoc)
            : base(id)
        {
            ItemId = itemId;
            Type = type;
            Quantity = quantity;
            Unit = unit;
            ReferenceDocument = referenceDoc;
            Timestamp = DateTime.UtcNow;
        }

#pragma warning disable CS8618
        private StockMutation() { }
#pragma warning restore CS8618

        public Guid ItemId { get; private set; }
        public MutationType Type { get; private set; }
        public decimal Quantity { get; private set; }
        public Unit Unit { get; private set; }
        public string ReferenceDocument { get; private set; }
        public DateTime Timestamp { get; private set; }

        public static StockMutation CreateIn(Guid itemId, decimal qty, Unit unit, string refDoc)
        {
            return new StockMutation(Guid.NewGuid(), itemId, MutationType.PurchaseIn, Math.Abs(qty), unit, refDoc);
        }

        public static StockMutation CreateOut(Guid itemId, decimal qty, Unit unit, string refDoc)
        {
            return new StockMutation(Guid.NewGuid(), itemId, MutationType.ProductionOut, -Math.Abs(qty), unit, refDoc);
        }
    }
}