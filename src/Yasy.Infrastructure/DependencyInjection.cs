using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Yasy.Core.Interfaces;
using Yasy.Infrastructure.Data;
using Yasy.Infrastructure.Data.Repositories;

namespace Yasy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<YasyDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
