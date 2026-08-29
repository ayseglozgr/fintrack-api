using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class ProxySettlementService : IProxySettlementService
{
    private readonly IProxySettlementRepository _proxySettlementRepository;

    public ProxySettlementService(IProxySettlementRepository proxySettlementRepository)
    {
        _proxySettlementRepository = proxySettlementRepository;
    }

    public async Task<ServiceResponse<IEnumerable<ProxySettlement>>> GetAllAsync()
    {
        var settlements = await _proxySettlementRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<ProxySettlement>>.Success(settlements);
    }

    public async Task<ServiceResponse<ProxySettlement>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<ProxySettlement>.Failure("Invalid proxy settlement id.");
        }

        var settlement = await _proxySettlementRepository.GetByIdAsync(id);
        if (settlement == null)
        {
            return ServiceResponse<ProxySettlement>.Failure("Proxy settlement not found.");
        }

        return ServiceResponse<ProxySettlement>.Success(settlement);
    }

    public async Task<ServiceResponse<ProxySettlement>> CreateAsync(ProxySettlement proxySettlement)
    {
        if (proxySettlement == null)
        {
            return ServiceResponse<ProxySettlement>.Failure("Proxy settlement payload is required.");
        }

        await _proxySettlementRepository.AddAsync(proxySettlement);
        return ServiceResponse<ProxySettlement>.Success(proxySettlement, "Proxy settlement created successfully.");
    }

    public async Task<ServiceResponse<ProxySettlement>> UpdateAsync(ProxySettlement proxySettlement)
    {
        if (proxySettlement == null || proxySettlement.Id <= 0)
        {
            return ServiceResponse<ProxySettlement>.Failure("Valid proxy settlement payload is required.");
        }

        var existing = await _proxySettlementRepository.GetByIdAsync(proxySettlement.Id);
        if (existing == null)
        {
            return ServiceResponse<ProxySettlement>.Failure("Proxy settlement not found.");
        }

        existing.ProxyCaseId = proxySettlement.ProxyCaseId;
        existing.SettlementDate = proxySettlement.SettlementDate;
        existing.Amount = proxySettlement.Amount;
        existing.PaymentChannel = proxySettlement.PaymentChannel;
        existing.EntryState = proxySettlement.EntryState;
        existing.ReceivedFinancialAccountId = proxySettlement.ReceivedFinancialAccountId;
        existing.SettlementTransactionId = proxySettlement.SettlementTransactionId;
        existing.Note = proxySettlement.Note;

        await _proxySettlementRepository.UpdateAsync(existing);
        return ServiceResponse<ProxySettlement>.Success(existing, "Proxy settlement updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid proxy settlement id.");
        }

        var existing = await _proxySettlementRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Proxy settlement not found.");
        }

        await _proxySettlementRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Proxy settlement deleted successfully.");
    }
}