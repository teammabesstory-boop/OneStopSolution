using System.Collections.Generic;
using OneStop.Domain.Common.Primitives;
using OneStop.Domain.Common.Shared; // <-- PENTING

namespace OneStop.Domain.Modules.Production
{
    public class Unit : ValueObject
    {
        public string Code { get; }
        public string Name { get; }
        public decimal ConversionFactorToBase { get; }

        private Unit(string code, string name, decimal conversionFactorToBase)
        {
            Code = code;
            Name = name;
            ConversionFactorToBase = conversionFactorToBase;
        }

        public static Unit Gram => new("GR", "Gram", 1);
        public static Unit Kilogram => new("KG", "Kilogram", 1000);
        public static Unit Liter => new("L", "Liter", 1000);
        public static Unit Milliliter => new("ML", "Milliliter", 1);
        public static Unit Pcs => new("PCS", "Pieces", 1);

        public static Result<Unit> CreateCustom(string code, string name, decimal factor)
        {
            if (string.IsNullOrWhiteSpace(code) || factor <= 0)
                return Result.Failure<Unit>(new Error("Unit.Invalid", "Invalid unit data"));

            return Result.Success(new Unit(code, name, factor));
        }

        public Result<decimal> ConvertTo(decimal amount, Unit targetUnit)
        {
            try
            {
                var baseAmount = amount * ConversionFactorToBase;
                var targetAmount = baseAmount / targetUnit.ConversionFactorToBase;
                return Result.Success(targetAmount);
            }
            catch
            {
                return Result.Failure<decimal>(new Error("Unit.ConversionFailed", "Division by zero or overflow"));
            }
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return ConversionFactorToBase;
        }
    }
}