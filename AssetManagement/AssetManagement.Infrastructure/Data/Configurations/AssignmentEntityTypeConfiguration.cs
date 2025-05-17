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
            .IsRequired()
            .HasColumnName("AssignedDate");

        builder.Property<string>(a => a.Note)
            .HasColumnName("Note");
        
        builder.Property<string>(a => a.AssignedBy)
            .HasColumnName("AssignedBy");
        
        builder.Property<string>(a => a.AssignedTo)
            .IsRequired()
            .HasColumnName("AssignedTo");

        builder.HasOne(a => a.Asset)
            .WithOne()
            .HasForeignKey<Assignment>(a => a.AssetCode)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>(a => a.AssignedToUser)
            .WithOne()
            .HasForeignKey<Assignment>(a => a.AssignedTo)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<User>(a => a.AssignedByUser)
            .WithOne()
            .HasForeignKey<Assignment>(a => a.AssignedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}