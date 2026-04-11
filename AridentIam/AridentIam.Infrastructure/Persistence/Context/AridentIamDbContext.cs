using AridentIam.Domain.Entities.Principals;
using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Context;

public sealed class AridentIamDbContext : DbContext
{
    public AridentIamDbContext(DbContextOptions<AridentIamDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Principal> Principals => Set<Principal>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AridentIamDbContext).Assembly);
    }
}