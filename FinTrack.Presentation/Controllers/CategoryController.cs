using FinTrack.Application.Common.Helpers;
using FinTrack.Application.DTOs.Category;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _categoryService.GetAllAsync();
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = response.Data.Select(MapCategory);
        return Ok(Application.Common.Models.ServiceResponse<IEnumerable<CategoryGetDto>>.Success(mapped));
    }

    [HttpGet("get-by-id/{uid}")]
    public async Task<IActionResult> GetById(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(Application.Common.Models.ServiceResponse<CategoryGetDto>.Failure("Invalid encrypted id."));
        }

        var response = await _categoryService.GetByIdAsync(id);
        if (!response.IsSuccess || response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(Application.Common.Models.ServiceResponse<CategoryGetDto>.Success(MapCategory(response.Data)));
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDto request)
    {
        var householdId = string.IsNullOrWhiteSpace(request.HouseholdUid) ? (int?)null : CipherHelper.DecryptId(request.HouseholdUid);
        var parentCategoryId = string.IsNullOrWhiteSpace(request.ParentCategoryUid) ? (int?)null : CipherHelper.DecryptId(request.ParentCategoryUid);

        if ((householdId.HasValue && householdId <= 0) || (parentCategoryId.HasValue && parentCategoryId <= 0))
        {
            return BadRequest(Application.Common.Models.ServiceResponse<CategoryGetDto>.Failure("Invalid uid value in request."));
        }

        var entity = new Category
        {
            HouseholdId = householdId,
            Name = request.Name,
            ParentCategoryId = parentCategoryId,
            Direction = request.Direction,
            IsActive = request.IsActive
        };

        var response = await _categoryService.CreateAsync(entity);
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = MapCategory(response.Data);
        return CreatedAtAction(nameof(GetById), new { uid = mapped.Uid }, Application.Common.Models.ServiceResponse<CategoryGetDto>.Success(mapped, response.Message));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryRequestDto request)
    {
        var id = CipherHelper.DecryptId(request.Uid);
        var householdId = string.IsNullOrWhiteSpace(request.HouseholdUid) ? (int?)null : CipherHelper.DecryptId(request.HouseholdUid);
        var parentCategoryId = string.IsNullOrWhiteSpace(request.ParentCategoryUid) ? (int?)null : CipherHelper.DecryptId(request.ParentCategoryUid);

        if (id <= 0 || (householdId.HasValue && householdId <= 0) || (parentCategoryId.HasValue && parentCategoryId <= 0))
        {
            return BadRequest(Application.Common.Models.ServiceResponse<CategoryGetDto>.Failure("Invalid uid value in request."));
        }

        var entity = new Category
        {
            Id = id,
            HouseholdId = householdId,
            Name = request.Name,
            ParentCategoryId = parentCategoryId,
            Direction = request.Direction,
            IsActive = request.IsActive
        };

        var response = await _categoryService.UpdateAsync(entity);
        if (!response.IsSuccess || response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(Application.Common.Models.ServiceResponse<CategoryGetDto>.Success(MapCategory(response.Data), response.Message));
    }

    [HttpDelete("delete/{uid}")]
    public async Task<IActionResult> Delete(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(Application.Common.Models.ServiceResponse.Failure("Invalid encrypted id."));
        }

        var response = await _categoryService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return NoContent();
    }

    private static CategoryGetDto MapCategory(Category entity)
    {
        return new CategoryGetDto
        {
            Uid = CipherHelper.EncryptId(entity.Id),
            HouseholdUid = entity.HouseholdId.HasValue ? CipherHelper.EncryptId(entity.HouseholdId.Value) : null,
            Name = entity.Name,
            ParentCategoryUid = entity.ParentCategoryId.HasValue ? CipherHelper.EncryptId(entity.ParentCategoryId.Value) : null,
            Direction = (int)entity.Direction,
            IsActive = entity.IsActive
        };
    }
}