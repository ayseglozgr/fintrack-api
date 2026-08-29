using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class ProxyCaseRepository : GenericRepository<ProxyCase>, IProxyCaseRepository
{
    public ProxyCaseRepository(FinTrackDbContext context) : base(context)
    {
    }
}