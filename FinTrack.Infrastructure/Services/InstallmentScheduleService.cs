using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class InstallmentScheduleService : IInstallmentScheduleService
{
    private readonly IInstallmentScheduleRepository _installmentScheduleRepository;

    public InstallmentScheduleService(IInstallmentScheduleRepository installmentScheduleRepository)
    {
        _installmentScheduleRepository = installmentScheduleRepository;
    }

    public async Task<ServiceResponse<IEnumerable<InstallmentSchedule>>> GetAllAsync()
    {
        var schedules = await _installmentScheduleRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<InstallmentSchedule>>.Success(schedules);
    }

    public async Task<ServiceResponse<InstallmentSchedule>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<InstallmentSchedule>.Failure("Invalid installment schedule id.");
        }

        var schedule = await _installmentScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
        {
            return ServiceResponse<InstallmentSchedule>.Failure("Installment schedule not found.");
        }

        return ServiceResponse<InstallmentSchedule>.Success(schedule);
    }

    public async Task<ServiceResponse<InstallmentSchedule>> CreateAsync(InstallmentSchedule installmentSchedule)
    {
        if (installmentSchedule == null)
        {
            return ServiceResponse<InstallmentSchedule>.Failure("Installment schedule payload is required.");
        }

        await _installmentScheduleRepository.AddAsync(installmentSchedule);
        return ServiceResponse<InstallmentSchedule>.Success(installmentSchedule, "Installment schedule created successfully.");
    }

    public async Task<ServiceResponse<InstallmentSchedule>> UpdateAsync(InstallmentSchedule installmentSchedule)
    {
        if (installmentSchedule == null || installmentSchedule.Id <= 0)
        {
            return ServiceResponse<InstallmentSchedule>.Failure("Valid installment schedule payload is required.");
        }

        var existing = await _installmentScheduleRepository.GetByIdAsync(installmentSchedule.Id);
        if (existing == null)
        {
            return ServiceResponse<InstallmentSchedule>.Failure("Installment schedule not found.");
        }

        existing.InstallmentPlanId = installmentSchedule.InstallmentPlanId;
        existing.InstallmentNo = installmentSchedule.InstallmentNo;
        existing.DueDate = installmentSchedule.DueDate;
        existing.Amount = installmentSchedule.Amount;
        existing.EntryState = installmentSchedule.EntryState;
        existing.StatusType = installmentSchedule.StatusType;
        existing.ActualTransactionId = installmentSchedule.ActualTransactionId;

        await _installmentScheduleRepository.UpdateAsync(existing);
        return ServiceResponse<InstallmentSchedule>.Success(existing, "Installment schedule updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid installment schedule id.");
        }

        var existing = await _installmentScheduleRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Installment schedule not found.");
        }

        await _installmentScheduleRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Installment schedule deleted successfully.");
    }
}