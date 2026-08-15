using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class FinancialAccountRepository : GenericRepository<FinancialAccount>, IFinancialAccountRepository
{
    public FinancialAccountRepository(FinTrackDbContext context) : base(context)
    {
    }
}
