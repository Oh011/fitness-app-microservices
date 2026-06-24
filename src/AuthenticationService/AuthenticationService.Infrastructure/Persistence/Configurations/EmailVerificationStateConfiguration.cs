using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationService.Infrastructure.Persistence.Configurations
{

    public class EmailVerificationStateConfiguration
        : IEntityTypeConfiguration<EmailVerificationState>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationState> builder)
        {
            builder.ToTable("EmailVerificationStates");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.ResendCount)
                .HasDefaultValue(0);

           
            builder.HasOne(x => x.user)
                .WithOne()
                .HasForeignKey<EmailVerificationState>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId)
                .IsUnique();
        }
    }
}
