using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class AssetEntityTypeConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");
        builder.HasKey(a => a.AssetCode);

        builder.Property(a => a.AssetCode)
            .IsRequired()
            .HasColumnName("AssetCode");
        
        builder.Property(a => a.AssetName)
            .HasColumnName("AssetName");
        
        builder.Property(a => a.Specification)
            .HasColumnName("Specification");

        builder.Property<AssetStatus>(a => a.State)
            .HasColumnName("Type");
        
        builder.Property<ELocation>(a => a.Location)
            .HasColumnName("Location");
        
        builder.Property<DateTime>(a => a.InstalledDate)
            .HasColumnName("InstalledDate");

        builder.HasOne(a => a.Category)
            .WithMany(c => c.Assets)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}