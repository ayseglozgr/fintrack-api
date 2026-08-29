using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface IProxyCaseService
{
    Task<ServiceResponse<IEnumerable<ProxyCase>>> GetAllAsync();
    Task<ServiceResponse<ProxyCase>> GetByIdAsync(int id);
    Task<ServiceResponse<ProxyCase>> CreateAsync(ProxyCase proxyCase);
    Task<ServiceResponse<ProxyCase>> UpdateAsync(ProxyCase proxyCase);
    Task<ServiceResponse> DeleteAsync(int id);
}