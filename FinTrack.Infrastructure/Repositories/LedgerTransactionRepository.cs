using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class LedgerTransactionRepository : GenericRepository<LedgerTransaction>, ILedgerTransactionRepository
{
    public LedgerTransactionRepository(FinTrackDbContext context) : base(context)
    {
    }
}