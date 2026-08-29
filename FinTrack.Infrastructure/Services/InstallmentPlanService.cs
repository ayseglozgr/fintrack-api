using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class InstallmentPlanService : IInstallmentPlanService
{
    private readonly IInstallmentPlanRepository _installmentPlanRepository;

    public InstallmentPlanService(IInstallmentPlanRepository installmentPlanRepository)
    {
        _installmentPlanRepository = installmentPlanRepository;
    }

    public async Task<ServiceResponse<IEnumerable<InstallmentPlan>>> GetAllAsync()
    {
        var plans = await _installmentPlanRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<InstallmentPlan>>.Success(plans);
    }

    public async Task<ServiceResponse<InstallmentPlan>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<InstallmentPlan>.Failure("Invalid installment plan id.");
        }

        var plan = await _installmentPlanRepository.GetByIdAsync(id);
        if (plan == null)
        {
            return ServiceResponse<InstallmentPlan>.Failure("Installment plan not found.");
        }

        return ServiceResponse<InstallmentPlan>.Success(plan);
    }

    public async Task<ServiceResponse<InstallmentPlan>> CreateAsync(InstallmentPlan installmentPlan)
    {
        if (installmentPlan == null)
        {
            return ServiceResponse<InstallmentPlan>.Failure("Installment plan payload is required.");
        }

        await _installmentPlanRepository.AddAsync(installmentPlan);
        return ServiceResponse<InstallmentPlan>.Success(installmentPlan, "Installment plan created successfully.");
    }

    public async Task<ServiceResponse<InstallmentPlan>> UpdateAsync(InstallmentPlan installmentPlan)
    {
        if (installmentPlan == null || installmentPlan.Id <= 0)
        {
            return ServiceResponse<InstallmentPlan>.Failure("Valid installment plan payload is required.");
        }

        var existing = await _installmentPlanRepository.GetByIdAsync(installmentPlan.Id);
        if (existing == null)
        {
            return ServiceResponse<InstallmentPlan>.Failure("Installment plan not found.");
        }

        existing.UserHouseholdId = installmentPlan.UserHouseholdId;
        existing.FinancialAccountId = installmentPlan.FinancialAccountId;
        existing.CategoryId = installmentPlan.CategoryId;
        existing.PurchaseDate = installmentPlan.PurchaseDate;
        existing.TotalAmount = installmentPlan.TotalAmount;
        existing.InstallmentCount = installmentPlan.InstallmentCount;
        existing.FirstDueDate = installmentPlan.FirstDueDate;
        existing.MerchantName = installmentPlan.MerchantName;
        existing.Description = installmentPlan.Description;

        await _installmentPlanRepository.UpdateAsync(existing);
        return ServiceResponse<InstallmentPlan>.Success(existing, "Installment plan updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid installment plan id.");
        }

        var existing = await _installmentPlanRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Installment plan not found.");
        }

        await _installmentPlanRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Installment plan deleted successfully.");
    }
}