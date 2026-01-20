// Path: src/Infrastructure/OneStop.Persistence/Configurations/IngredientConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneStop.Domain.Modules.Production;

namespace OneStop.Persistence.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Sku)
                .HasMaxLength(50)
                .IsRequired();

            // Index pada SKU agar pencarian cepat & unik
            builder.HasIndex(x => x.Sku).IsUnique();

            builder.Property(x => x.CurrentPricePerUnit)
                .HasColumnType("decimal(18,2)");

            // VALUE OBJECT MAPPING (Flattening)
            // Kolom di DB akan jadi: StockUnit_Code, StockUnit_Name, StockUnit_Factor
            builder.OwnsOne(x => x.StockUnit, unitBuilder =>
            {
                unitBuilder.Property(u => u.Code).HasColumnName("StockUnit_Code").HasMaxLength(10);
                unitBuilder.Property(u => u.Name).HasColumnName("StockUnit_Name").HasMaxLength(50);
                unitBuilder.Property(u => u.ConversionFactorToBase).HasColumnName("StockUnit_Factor").HasColumnType("decimal(18,4)");
            });
        }
    }
}