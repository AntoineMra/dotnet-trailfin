using Microsoft.EntityFrameworkCore;
using Trailfin.Domain.Entities;
using Trailfin.Domain.Entitites;

namespace Trailfin.Infrastructure.Persistence;

public class TrailfinContext : DbContext
{
    public TrailfinContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("user");
        builder.Entity<User>().HasKey(x => x.Id);
        builder.Entity<Trip>().ToTable("trip");
        builder.Entity<Trip>().HasKey(x => x.Id);
    }
}