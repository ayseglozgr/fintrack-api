using FinTrack.Application.Common.Models;
using FinTrack.Domain.Entities;

namespace FinTrack.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<ServiceResponse<IEnumerable<Category>>> GetAllAsync();
    Task<ServiceResponse<Category>> GetByIdAsync(int id);
    Task<ServiceResponse<Category>> CreateAsync(Category category);
    Task<ServiceResponse<Category>> UpdateAsync(Category category);
    Task<ServiceResponse> DeleteAsync(int id);
}