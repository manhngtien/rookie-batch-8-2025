using AssetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CategoryName)
            .IsRequired()
            .HasColumnName("CategoryName");
        
        builder.Property(a => a.Prefix)
            .IsRequired()
            .HasColumnName("Prefix");

        builder.Property(a => a.Total)
            .HasColumnName("Total")
            .HasDefaultValue(0);
        
        builder.HasMany(c => c.Assets)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}