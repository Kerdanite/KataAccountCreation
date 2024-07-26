using AccountManagement.Domain.Account;
using AccountManagement.Infrastructure.EFRepositories;
using AccountManagement.Infrastructure.InMemoryRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AccountManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        //RegisterInMemoryRepositories(services);
        RegisterEFRepositories(services);
        return services;
    }

    private static void RegisterInMemoryRepositories(IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, AccountInMemoryRepository>();
    }private static void RegisterEFRepositories(IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
    }
}