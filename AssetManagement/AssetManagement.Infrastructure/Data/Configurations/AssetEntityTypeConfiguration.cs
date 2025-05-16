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
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AssetCode)
            .IsRequired()
            .HasColumnName("AssetCode");
        
        builder.Property(a => a.AssetName)
            .HasColumnName("AssetName");
        
        builder.Property(a => a.Specification)
            .HasColumnName("Specification");

        builder.Property<AssetStatus>(u => u.State)
            .HasColumnName("Type");
        
        builder.Property<ELocation>(u => u.Location)
            .HasColumnName("Location");
        
        builder.Property<DateTime>(u => u.InstalledDate)
            .HasColumnName("InstalledDate");

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}