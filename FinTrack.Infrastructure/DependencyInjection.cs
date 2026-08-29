using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Infrastructure.Factories;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? envString = configuration["Environment"];

        Enum.TryParse<Domain.Const.Enum.Environment>(envString, true, out var currentEnvironment);

        string? connectionString = configuration.GetConnectionString("ConnectionString_" + currentEnvironment.ToString())
                                   ?? configuration.GetConnectionString("ConnectionString_TEST");

        services.AddDbContext<FinTrackDbContext>(options =>
            options.UseNpgsql(connectionString));

        //repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHouseholdRepository, HouseholdRepository>();
        services.AddScoped<IUserHouseholdRepository, UserHouseholdRepository>();
        services.AddScoped<IFinancialAccountRepository, FinancialAccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ILedgerTransactionRepository, LedgerTransactionRepository>();
        services.AddScoped<IInstallmentPlanRepository, InstallmentPlanRepository>();
        services.AddScoped<IInstallmentScheduleRepository, InstallmentScheduleRepository>();
        services.AddScoped<IProxyCaseRepository, ProxyCaseRepository>();
        services.AddScoped<IProxySettlementRepository, ProxySettlementRepository>();

        //services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserHouseHoldFactory, UserHouseHoldFactory>();
        services.AddScoped<IHouseholdService, HouseholdService>();
        services.AddScoped<IFinancialAccountService, FinancialAccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ILedgerTransactionService, LedgerTransactionService>();
        services.AddScoped<IInstallmentPlanService, InstallmentPlanService>();
        services.AddScoped<IInstallmentScheduleService, InstallmentScheduleService>();
        services.AddScoped<IProxyCaseService, ProxyCaseService>();
        services.AddScoped<IProxySettlementService, ProxySettlementService>();

        return services;
    }
}