using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface IInstallmentScheduleService
{
    Task<ServiceResponse<IEnumerable<InstallmentSchedule>>> GetAllAsync();
    Task<ServiceResponse<InstallmentSchedule>> GetByIdAsync(int id);
    Task<ServiceResponse<InstallmentSchedule>> CreateAsync(InstallmentSchedule installmentSchedule);
    Task<ServiceResponse<InstallmentSchedule>> UpdateAsync(InstallmentSchedule installmentSchedule);
    Task<ServiceResponse> DeleteAsync(int id);
}