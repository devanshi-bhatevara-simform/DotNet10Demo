using DotNet10Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet10Demo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasQueryFilter("SoftDelete", p => !p.IsDeleted)
            .HasQueryFilter("ActiveOnly", p => p.IsActive);

        // ToJson() stores ProductDetails as a JSON column instead of separate columns
        modelBuilder.Entity<Product>().OwnsOne(p => p.Details, d => d.ToJson());
    }
}
