using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ServiceResponse<IEnumerable<Category>>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<Category>>.Success(categories);
    }

    public async Task<ServiceResponse<Category>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<Category>.Failure("Invalid category id.");
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return ServiceResponse<Category>.Failure("Category not found.");
        }

        return ServiceResponse<Category>.Success(category);
    }

    public async Task<ServiceResponse<Category>> CreateAsync(Category category)
    {
        if (category == null)
        {
            return ServiceResponse<Category>.Failure("Category payload is required.");
        }

        await _categoryRepository.AddAsync(category);
        return ServiceResponse<Category>.Success(category, "Category created successfully.");
    }

    public async Task<ServiceResponse<Category>> UpdateAsync(Category category)
    {
        if (category == null || category.Id <= 0)
        {
            return ServiceResponse<Category>.Failure("Valid category payload is required.");
        }

        var existing = await _categoryRepository.GetByIdAsync(category.Id);
        if (existing == null)
        {
            return ServiceResponse<Category>.Failure("Category not found.");
        }

        existing.HouseholdId = category.HouseholdId;
        existing.Name = category.Name;
        existing.ParentCategoryId = category.ParentCategoryId;
        existing.Direction = category.Direction;
        existing.IsActive = category.IsActive;

        await _categoryRepository.UpdateAsync(existing);
        return ServiceResponse<Category>.Success(existing, "Category updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid category id.");
        }

        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Category not found.");
        }

        await _categoryRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Category deleted successfully.");
    }
}