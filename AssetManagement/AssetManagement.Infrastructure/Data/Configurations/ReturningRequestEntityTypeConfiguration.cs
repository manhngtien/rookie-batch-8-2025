using AssetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class ReturningRequestEntityTypeConfiguration : IEntityTypeConfiguration<ReturningRequest>
{
    public void Configure(EntityTypeBuilder<ReturningRequest> builder)
    {
        builder.ToTable("ReturningRequests");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.State)
            .HasColumnName("State");

        builder.Property(r => r.ReturnedDate)
            .HasColumnName("ReturnedDate");

        builder.Property<string>(r => r.RequestedBy)
            .IsRequired()
            .HasColumnName("RequestedBy");

        builder.Property<string>(r => r.AcceptedBy)
            .HasColumnName("AcceptedBy");

        builder.Property(r => r.AssignmentId)
            .IsRequired()
            .HasColumnName("AssignmentId");
            
        builder.HasOne<User>(r => r.RequestedByUser)
            .WithOne()
            .HasForeignKey<ReturningRequest>(r => r.RequestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>(r => r.AcceptedByUser)
            .WithOne()
            .HasForeignKey<ReturningRequest>(r => r.AcceptedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Assignment>(r => r.Assignment)
            .WithOne()
            .HasForeignKey<ReturningRequest>(r => r.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}