using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IHouseholdRepository : IGenericRepository<Household>
{
    // İleride haneye özel (örneğin hane detayını kullanıcılarıyla getiren) 
    // spesifik sorgular gerekirse buraya eklenecek. Şimdilik generic yapı yeterli.
}