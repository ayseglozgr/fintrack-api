using FinTrack.Application.Interfaces.Services;
using FinTrack.Application.DTOs.FinancialAccount;
using Microsoft.AspNetCore.Mvc;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinancialAccountController : ControllerBase
{
    private readonly IFinancialAccountService _financialAccountService;

    public FinancialAccountController(IFinancialAccountService financialAccountService)
    {
        _financialAccountService = financialAccountService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] FinancialAccountType? type)
    {
        var response = await _financialAccountService.GetAllAsync(type);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("get-by-id/{uid}")]
    public async Task<IActionResult> GetById(string uid)
    {
        var response = await _financialAccountService.GetByIdAsync(uid);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateFinancialAccountRequestDto request)
    {
        var response = await _financialAccountService.CreateAsync(request);
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetById), new { uid = response.Data.Uid }, response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateFinancialAccountRequestDto request)
    {
        var response = await _financialAccountService.UpdateAsync(request);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpDelete("delete/{uid}")]
    public async Task<IActionResult> Delete(string uid)
    {
        var response = await _financialAccountService.DeleteAsync(uid);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return NoContent();
    }
}
