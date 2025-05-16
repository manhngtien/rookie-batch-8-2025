using AssetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.Property(a => a.StaffCode)
            .IsRequired()
            .HasColumnName("StaffCode");
        
        builder.Property(a => a.CreatedDate)
            .HasColumnName("CreatedDate");
        
        builder.Property(a => a.UpdatedDate)
            .HasColumnName("UpdatedDate");

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Account>(a => a.StaffCode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}