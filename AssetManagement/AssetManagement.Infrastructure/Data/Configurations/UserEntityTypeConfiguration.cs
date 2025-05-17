using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManagement.Infrastructure.Data.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.StaffCode);

        builder.Property(u => u.StaffCode)
            .IsRequired()
            .HasColumnName("StaffCode");

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasColumnName("UserName");
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(128)
            .HasColumnName("FirstName");
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(128)
            .HasColumnName("LastName");
        
        builder.Property<DateTime>(u => u.DateOfBirth)
            .IsRequired()
            .HasColumnName("DateOfBirth");
        
        builder.Property<DateTime>(u => u.JoinedDate)
            .IsRequired()
            .HasColumnName("JoinedDate");
        
        builder.Property<bool>(u => u.Gender)
            .HasColumnName("Gender");

        builder.Property<ERole>(u => u.Type)
            .HasColumnName("Type")
            .HasDefaultValue(ERole.Staff);

        builder.Property<ELocation>(u => u.Location)
            .IsRequired()
            .HasColumnName("Location");

        builder.Property<bool>(u => u.IsDisabled)
            .HasColumnName("IsDisabled");
    }
}