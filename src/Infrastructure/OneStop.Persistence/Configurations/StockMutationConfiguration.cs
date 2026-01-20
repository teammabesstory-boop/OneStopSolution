// Path: src/Infrastructure/OneStop.Persistence/Configurations/StockMutationConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneStop.Domain.Modules.Inventory;

namespace OneStop.Persistence.Configurations
{
    public class StockMutationConfiguration : IEntityTypeConfiguration<StockMutation>
    {
        public void Configure(EntityTypeBuilder<StockMutation> builder)
        {
            builder.ToTable("StockMutations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity).HasColumnType("decimal(18,4)");
            builder.Property(x => x.ReferenceDocument).HasMaxLength(100);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.OwnsOne(x => x.Unit, unitBuilder =>
            {
                unitBuilder.Property(u => u.Code).HasColumnName("Unit_Code").HasMaxLength(10);
                unitBuilder.Property(u => u.Name).HasColumnName("Unit_Name").HasMaxLength(50);
                unitBuilder.Property(u => u.ConversionFactorToBase).HasColumnName("Unit_Factor").HasColumnType("decimal(18,4)");
            });

            // Index untuk mempercepat query history per item
            builder.HasIndex(x => x.ItemId);
            builder.HasIndex(x => x.Timestamp);
        }
    }
}