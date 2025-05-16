using AssetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AssetCode)
            .IsRequired()
            .HasColumnName("AssetCode");
        
        builder.Property(a => a.State)
            .HasColumnName("State");
        
        builder.Property(a => a.AssignedDate)
            .HasColumnName("AssignedDate");

        builder.Property<string>(u => u.Note)
            .HasColumnName("Note");
        
        builder.Property<Guid>(u => u.AssignBy)
            .HasColumnName("AssignBy");
        
        builder.Property<Guid>(u => u.AssignedTo)
            .HasColumnName("AssignedTo");

        builder.HasOne<Asset>()
            .WithOne()
            .HasForeignKey<Assignment>(a => a.AssetCode);
    }
}