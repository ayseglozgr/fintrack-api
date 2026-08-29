using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface ILedgerTransactionService
{
    Task<ServiceResponse<IEnumerable<LedgerTransaction>>> GetAllAsync();
    Task<ServiceResponse<LedgerTransaction>> GetByIdAsync(int id);
    Task<ServiceResponse<LedgerTransaction>> CreateAsync(LedgerTransaction transaction);
    Task<ServiceResponse<LedgerTransaction>> UpdateAsync(LedgerTransaction transaction);
    Task<ServiceResponse> DeleteAsync(int id);
}