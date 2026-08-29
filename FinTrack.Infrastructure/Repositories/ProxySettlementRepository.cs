using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class ProxySettlementRepository : GenericRepository<ProxySettlement>, IProxySettlementRepository
{
    public ProxySettlementRepository(FinTrackDbContext context) : base(context)
    {
    }
}