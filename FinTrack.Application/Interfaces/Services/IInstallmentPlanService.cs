using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface IInstallmentPlanService
{
    Task<ServiceResponse<IEnumerable<InstallmentPlan>>> GetAllAsync();
    Task<ServiceResponse<InstallmentPlan>> GetByIdAsync(int id);
    Task<ServiceResponse<InstallmentPlan>> CreateAsync(InstallmentPlan installmentPlan);
    Task<ServiceResponse<InstallmentPlan>> UpdateAsync(InstallmentPlan installmentPlan);
    Task<ServiceResponse> DeleteAsync(int id);
}