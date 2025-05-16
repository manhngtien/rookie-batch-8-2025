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
        
        builder.Property<Guid>(u => u.RequestedBy)
            .HasColumnName("RequestedBy");
        
        builder.Property<Guid>(u => u.AcceptedBy)
            .HasColumnName("AcceptedBy");

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<ReturningRequest>(r => r.RequestedBy);
        
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<ReturningRequest>(r => r.AcceptedBy);
    }
}