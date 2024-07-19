using AccountManagement.Domain.Account;
using AccountManagement.Infrastructure.InMemoryRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        RegisterInMemoryRepositories(services);
        return services;
    }

    private static void RegisterInMemoryRepositories(IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, AccountInMemoryRepository>();
    }
}