using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface IProxySettlementService
{
    Task<ServiceResponse<IEnumerable<ProxySettlement>>> GetAllAsync();
    Task<ServiceResponse<ProxySettlement>> GetByIdAsync(int id);
    Task<ServiceResponse<ProxySettlement>> CreateAsync(ProxySettlement proxySettlement);
    Task<ServiceResponse<ProxySettlement>> UpdateAsync(ProxySettlement proxySettlement);
    Task<ServiceResponse> DeleteAsync(int id);
}