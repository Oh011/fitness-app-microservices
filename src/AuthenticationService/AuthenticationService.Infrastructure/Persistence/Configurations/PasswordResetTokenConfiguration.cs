using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class PasswordResetTokenConfiguration
        : IEntityTypeConfiguration<PasswordResetTokens>
    {
        public void Configure(EntityTypeBuilder<PasswordResetTokens> builder)
        {
            builder.ToTable("PasswordResetTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .HasDefaultValue(false);


            builder.HasIndex(x => x.TokenHash)
       .IsUnique();


            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.user)
                .WithMany(a=>a.PasswordResetTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
