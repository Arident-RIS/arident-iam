using AridentIam.Domain.Common;
using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Entities.Principals;
using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Context;

public sealed class AridentIamDbContext : DbContext
{
    private readonly IMediator? _mediator;

    // Used by DI (runtime)
    public AridentIamDbContext(DbContextOptions<AridentIamDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    // Used by design-time factory (EF migrations)
    public AridentIamDbContext(DbContextOptions<AridentIamDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantSetting> TenantSettings => Set<TenantSetting>();
    public DbSet<Principal> Principals => Set<Principal>();
    public DbSet<User> Users => Set<User>();
    public DbSet<OrgSchema> OrgSchemas => Set<OrgSchema>();
    public DbSet<OrgUnitType> OrgUnitTypes => Set<OrgUnitType>();
    public DbSet<OrgUnit> OrgUnits => Set<OrgUnit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AridentIamDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect and clear domain events before saving so recursive saves don't re-dispatch.
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        ChangeTracker
            .Entries<BaseEntity>()
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_mediator is not null)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish((object)domainEvent, cancellationToken);
            }
        }

        return result;
    }
}