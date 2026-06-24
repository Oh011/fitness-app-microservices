using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(r => r.Id);

            builder.ToTable("Roles");

            // 🔹 Name
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(r => r.Name)
                .IsUnique(); // role names should be unique

            // 🔹 Description
            builder.Property(r => r.Description)
                .HasMaxLength(250);

            // 🔹 Users Navigation
            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role)   // navigation in ApplicationUser
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull); // deleting a role sets RoleId = null
        }

    }
}
