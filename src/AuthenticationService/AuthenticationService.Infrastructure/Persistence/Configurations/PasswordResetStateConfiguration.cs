using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{

    public class PasswordResetStateConfiguration
        : IEntityTypeConfiguration<PasswordResetState>
    {
        public void Configure(EntityTypeBuilder<PasswordResetState> builder)
        {
            builder.ToTable("PasswordResetStates");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.ResendCount)
                .HasDefaultValue(0);

            builder.HasOne(x => x.user)
                .WithOne()
                .HasForeignKey<PasswordResetState>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId)
                .IsUnique();
        }
    }
}
