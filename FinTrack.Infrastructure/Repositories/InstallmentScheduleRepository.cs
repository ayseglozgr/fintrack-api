using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class InstallmentScheduleRepository : GenericRepository<InstallmentSchedule>, IInstallmentScheduleRepository
{
    public InstallmentScheduleRepository(FinTrackDbContext context) : base(context)
    {
    }
}