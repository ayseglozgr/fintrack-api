using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.FinancialAccount;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Application.Interfaces.Services;

public interface IFinancialAccountService
{
    Task<ServiceResponse<IEnumerable<FinancialAccountGetDto>>> GetAllAsync(FinancialAccountType? type = null);
    Task<ServiceResponse<FinancialAccountGetDto>> GetByIdAsync(string uid);
    Task<ServiceResponse<FinancialAccountGetDto>> CreateAsync(CreateFinancialAccountRequestDto request);
    Task<ServiceResponse<FinancialAccountGetDto>> UpdateAsync(UpdateFinancialAccountRequestDto request);
    Task<ServiceResponse> DeleteAsync(string uid);
}
