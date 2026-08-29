using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class ProxyCaseService : IProxyCaseService
{
    private readonly IProxyCaseRepository _proxyCaseRepository;

    public ProxyCaseService(IProxyCaseRepository proxyCaseRepository)
    {
        _proxyCaseRepository = proxyCaseRepository;
    }

    public async Task<ServiceResponse<IEnumerable<ProxyCase>>> GetAllAsync()
    {
        var proxyCases = await _proxyCaseRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<ProxyCase>>.Success(proxyCases);
    }

    public async Task<ServiceResponse<ProxyCase>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<ProxyCase>.Failure("Invalid proxy case id.");
        }

        var proxyCase = await _proxyCaseRepository.GetByIdAsync(id);
        if (proxyCase == null)
        {
            return ServiceResponse<ProxyCase>.Failure("Proxy case not found.");
        }

        return ServiceResponse<ProxyCase>.Success(proxyCase);
    }

    public async Task<ServiceResponse<ProxyCase>> CreateAsync(ProxyCase proxyCase)
    {
        if (proxyCase == null)
        {
            return ServiceResponse<ProxyCase>.Failure("Proxy case payload is required.");
        }

        await _proxyCaseRepository.AddAsync(proxyCase);
        return ServiceResponse<ProxyCase>.Success(proxyCase, "Proxy case created successfully.");
    }

    public async Task<ServiceResponse<ProxyCase>> UpdateAsync(ProxyCase proxyCase)
    {
        if (proxyCase == null || proxyCase.Id <= 0)
        {
            return ServiceResponse<ProxyCase>.Failure("Valid proxy case payload is required.");
        }

        var existing = await _proxyCaseRepository.GetByIdAsync(proxyCase.Id);
        if (existing == null)
        {
            return ServiceResponse<ProxyCase>.Failure("Proxy case not found.");
        }

        existing.UserHouseholdId = proxyCase.UserHouseholdId;
        existing.FinancialAccountId = proxyCase.FinancialAccountId;
        existing.ExternalPartyName = proxyCase.ExternalPartyName;
        existing.SpentDate = proxyCase.SpentDate;
        existing.TotalAdvancedAmount = proxyCase.TotalAdvancedAmount;
        existing.Status = proxyCase.Status;
        existing.ClosedDate = proxyCase.ClosedDate;
        existing.Description = proxyCase.Description;

        await _proxyCaseRepository.UpdateAsync(existing);
        return ServiceResponse<ProxyCase>.Success(existing, "Proxy case updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid proxy case id.");
        }

        var existing = await _proxyCaseRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Proxy case not found.");
        }

        await _proxyCaseRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Proxy case deleted successfully.");
    }
}