#define dds_tests

using Booking.Auth.Application.Repositories;
using Booking.Auth.Persistence.Context;
using Booking.Auth.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Auth.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
#if dds_tests
        var connectionString = configuration.GetConnectionString("PostgreSQLdds");
#else
        var connectionString = configuration.GetConnectionString("PostgreSQL");
#endif


        services.AddDbContext<DataContext>(opt => { opt.UseNpgsql(connectionString); });
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICompanyRepository, CompanyRepository>();
        services.AddTransient<IFilialRepository, FilialRepository>();
    }
}