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

            // BARU: Konfigurasi untuk properti Stock (Wajib di-define presisinya)
            builder.Property(x => x.CurrentStock)
                .HasColumnType("decimal(18,2)"); // Misal: 1.50 Kg

            builder.Property(x => x.MinimumStock)
                .HasColumnType("decimal(18,2)");

            // UPDATE: Mapping Unit (sebelumnya StockUnit)
            // Ganti 'x.StockUnit' menjadi 'x.Unit' sesuai class Ingredient.cs
            builder.OwnsOne(x => x.Unit, unitBuilder =>
            {
                // Kita ganti nama kolom di DB jadi 'Unit_...' biar lebih konsisten
                unitBuilder.Property(u => u.Code).HasColumnName("Unit_Code").HasMaxLength(10);
                unitBuilder.Property(u => u.Name).HasColumnName("Unit_Name").HasMaxLength(50);
                unitBuilder.Property(u => u.ConversionFactorToBase).HasColumnName("Unit_Factor").HasColumnType("decimal(18,4)");
            });
        }
    }
}