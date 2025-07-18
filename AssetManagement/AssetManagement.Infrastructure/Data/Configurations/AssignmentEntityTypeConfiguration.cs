﻿using AssetManagement.Core.Entities;
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

        builder.Property<int?>(a => a.ReturningRequestId)
            .HasColumnName("ReturningRequestId");

        builder.HasOne<ReturningRequest>(a => a.ReturningRequest)
            .WithOne(r => r.Assignment)
            .HasForeignKey<Assignment>(a => a.ReturningRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Asset)
            .WithMany(a => a.Assignments)
            .HasForeignKey(a => a.AssetCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>(a => a.AssignedToUser)
            .WithMany()
            .HasForeignKey(a => a.AssignedTo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>(a => a.AssignedByUser)
            .WithMany()
            .HasForeignKey(a => a.AssignedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}