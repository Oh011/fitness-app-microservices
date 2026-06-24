
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace  AuthenticationService.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext:DbContext
    {



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }


        public DbSet<ApplicationUser> Users { get; set; }





        public DbSet<Role> Roles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public DbSet<EmailConfirmationTokens> EmailConfirmationTokens { get; set; }


        public DbSet<PasswordResetTokens> PasswordResetTokens { get; set; }


        public DbSet<PasswordResetState> passwordResetStates { get; set; }
        public DbSet<EmailVerificationState> EmailVerificationsStates { get; set; }


    }
}
