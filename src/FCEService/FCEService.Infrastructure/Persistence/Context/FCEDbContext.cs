using FCEService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Infrastructure.Persistence.Context;

public class FCEDbContext : DbContext
{
    public FCEDbContext(DbContextOptions<FCEDbContext> options) : base(options)
    {
    }

    public DbSet<UserFitnessStats> UserFitnessStats { get; set; }
    public DbSet<CalculatedMetrics> CalculatedMetrics { get; set; }
    public DbSet<FitnessPlanConfig> FitnessPlanConfigs { get; set; }
    public DbSet<UserAssignedPlan> UserAssignedPlans { get; set; }
    public DbSet<UserPlanHistory> UserPlanHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCEDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
