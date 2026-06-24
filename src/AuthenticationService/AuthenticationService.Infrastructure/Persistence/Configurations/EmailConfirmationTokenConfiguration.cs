using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AuthenticationService.Infrastructure.Persistence.Configurations
{
    public class EmailConfirmationTokenConfiguration
        : IEntityTypeConfiguration<EmailConfirmationTokens>
    {
        public void Configure(EntityTypeBuilder<EmailConfirmationTokens> builder)
        {
            builder.ToTable("EmailConfirmationTokens");

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
                .WithMany(a => a.EmailconfirmationTokens)               
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
