using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. PostgreSQL DbContext Bağımlılığının Eklenmesi
        // Bağlantı cümlesini (Connection String) API'deki appsettings.json'dan okuyacak
        services.AddDbContext<FinTrackDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // 2. Repository Bağımlılıklarının (DI) Tanımlanması (Scoped asenkron süreçler için en doğrusudur)
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHouseholdRepository, HouseholdRepository>();

        return services;
    }
}