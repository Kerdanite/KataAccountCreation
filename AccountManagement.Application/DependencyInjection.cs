using AccountManagement.Application.Abstractions.Behaviors;
using AccountManagement.Domain.Account;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddTransient<AccountService>();

        return services;
    }
}