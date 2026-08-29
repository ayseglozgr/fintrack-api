using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class InstallmentPlanRepository : GenericRepository<InstallmentPlan>, IInstallmentPlanRepository
{
    public InstallmentPlanRepository(FinTrackDbContext context) : base(context)
    {
    }
}