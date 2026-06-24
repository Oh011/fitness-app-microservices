using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{



    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // 🔹 Table name
            builder.ToTable("RefreshTokens");

            // 🔹 Primary Key
            builder.HasKey(rt => rt.Id);

            // 🔹 Token
            builder.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(200);

            // 🔹 DeviceId
            builder.Property(rt => rt.DeviceId)
                .IsRequired()
                .HasMaxLength(100);

            // 🔹 UserId foreign key
            builder.Property(rt => rt.UserId)
                .IsRequired();

            // 🔹 Dates
            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .IsRequired(false);

            // 🔹 Flags
            builder.Property(rt => rt.IsRevoked)
                .IsRequired();

            // 🔹 Relationships
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.refreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Indexes (optional but recommended)




            // 1. Token validation (critical path)
            builder.HasIndex(x => x.TokenHash)
                   .IsUnique();

            // 2. Per-device token management
            builder.HasIndex(x => new
            {
                x.UserId,
                x.DeviceId,
                x.IsRevoked
            });

            // 3. User-wide token management
            builder.HasIndex(x => new
            {
                x.UserId,
                x.IsRevoked,
                x.ExpiresAt
            });
        }
    }

}
