using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Domain.Entities;

namespace StealAllTheCats.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public DbSet<CatEntity> Cats { get; set; }
    public DbSet<TagEntity> Tags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatTag>().HasKey(ct => new { ct.CatId, ct.TagId });
    }
}