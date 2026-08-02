using BackendDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendDotnet.Data;

public sealed class InitiativesDbContext(DbContextOptions<InitiativesDbContext> options) : DbContext(options)
{
    public DbSet<Initiative> Initiatives => Set<Initiative>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Initiative>(entity =>
        {
            entity.ToTable("initiatives");
            entity.HasKey(initiative => initiative.Id);
            entity.Property(initiative => initiative.Id).HasColumnName("id");
            entity.Property(initiative => initiative.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(initiative => initiative.Description).HasColumnName("description").IsRequired();
            entity.Property(initiative => initiative.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
            entity.Property(initiative => initiative.BusinessProblem).HasColumnName("business_problem");
            entity.Property(initiative => initiative.ExpectedBenefit).HasColumnName("expected_benefit");
            entity.Property(initiative => initiative.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(initiative => initiative.AnalysisResult).HasColumnName("analysis_result");
        });
    }
}
