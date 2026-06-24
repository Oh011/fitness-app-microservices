using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {


            builder.HasKey(u => u.Id);

            builder.ToTable("ApplicationUsers");

            // 🔒 Email
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique(); // Email must be unique

            // 🔐 Name
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            // 🔑 Password
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.PasswordSalt)
                .IsRequired()
                .HasMaxLength(256);

            // 🔒 Role
            builder.HasOne(u => u.Role)
                .WithMany() // Role may have many users
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull); // Optional: if role deleted, set RoleId to null

            // ⚠️ Account Locking / Retry
            builder.Property(u => u.IsLocked)
                .HasDefaultValue(false);

            builder.Property(u => u.RetryCount)
                .HasDefaultValue(0);

            // 🔄 Refresh Tokens
            builder.HasMany(u => u.refreshTokens)
                .WithOne(rt => rt.User) // you need a navigation in RefreshToken: ApplicationUser
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade); // delete refresh tokens if user deleted
        }
    }
}
