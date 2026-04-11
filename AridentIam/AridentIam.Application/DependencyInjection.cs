using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AridentIam.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(Behaviors.TransactionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}