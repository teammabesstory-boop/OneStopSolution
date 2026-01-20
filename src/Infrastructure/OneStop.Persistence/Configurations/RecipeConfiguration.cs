// Path: src/Infrastructure/OneStop.Persistence/Configurations/RecipeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneStop.Domain.Modules.Production;

namespace OneStop.Persistence.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.OutputQuantity).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CachedCostPerUnit).HasColumnType("decimal(18,2)");

            // Map Value Object OutputUnit
            builder.OwnsOne(x => x.OutputUnit, unitBuilder =>
            {
                unitBuilder.Property(u => u.Code).HasColumnName("OutputUnit_Code").HasMaxLength(10);
                unitBuilder.Property(u => u.Name).HasColumnName("OutputUnit_Name").HasMaxLength(50);
                unitBuilder.Property(u => u.ConversionFactorToBase).HasColumnName("OutputUnit_Factor").HasColumnType("decimal(18,4)");
            });

            // OWNED COLLECTION MAPPING (Recipe Items)
            // Ini akan membuat tabel otomatis bernama "RecipeItems"
            builder.OwnsMany(x => x.Items, itemBuilder =>
            {
                itemBuilder.ToTable("RecipeItems");

                // Foreign Key otomatis ke RecipeId
                itemBuilder.WithOwner().HasForeignKey("RecipeId");

                itemBuilder.Property(i => i.ComponentId).IsRequired();

                // Simpan Enum sebagai String agar mudah dibaca di DB
                itemBuilder.Property(i => i.Type)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                itemBuilder.Property(i => i.Quantity).HasColumnType("decimal(18,4)");

                // Map Unit di dalam RecipeItem
                itemBuilder.OwnsOne(i => i.Unit, uBuilder =>
                {
                    uBuilder.Property(u => u.Code).HasColumnName("Unit_Code").HasMaxLength(10);
                    uBuilder.Property(u => u.Name).HasColumnName("Unit_Name").HasMaxLength(50);
                    uBuilder.Property(u => u.ConversionFactorToBase).HasColumnName("Unit_Factor").HasColumnType("decimal(18,4)");
                });
            });

            // Akses field private _items melalui Property Access Mode
            builder.Metadata.FindNavigation(nameof(Recipe.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}