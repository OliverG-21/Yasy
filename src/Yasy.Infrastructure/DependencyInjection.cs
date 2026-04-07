using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Yasy.Core.Interfaces;
using Yasy.Infrastructure.Data;
using Yasy.Infrastructure.Data.Repositories;
using Yasy.Infrastructure.Services;

namespace Yasy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string dataPath)
    {
        services.AddDbContext<YasyDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseEnrollmentRepository, CourseEnrollmentRepository>();
        services.AddScoped<ICourseCardRepository, CourseCardRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddSingleton<IProfileImageService>(new ProfileImageService(dataPath));

        return services;
    }
}
