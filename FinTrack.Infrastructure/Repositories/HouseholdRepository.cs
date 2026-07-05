using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;

namespace FinTrack.Infrastructure.Repositories;

public class HouseholdRepository : GenericRepository<Household>, IHouseholdRepository
{
    public HouseholdRepository(FinTrackDbContext context) : base(context)
    {
    }
}