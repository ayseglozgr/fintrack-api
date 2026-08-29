using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Installment;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstallmentController : ControllerBase
{
    private readonly ITransactionFactory _transactionFactory;
    private readonly IInstallmentPlanService _installmentPlanService;
    private readonly IInstallmentScheduleService _installmentScheduleService;

    public InstallmentController(
        ITransactionFactory transactionFactory,
        IInstallmentPlanService installmentPlanService,
        IInstallmentScheduleService installmentScheduleService)
    {
        _transactionFactory = transactionFactory;
        _installmentPlanService = installmentPlanService;
        _installmentScheduleService = installmentScheduleService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var plansResponse = await _installmentPlanService.GetAllAsync();
        if (!plansResponse.IsSuccess || plansResponse.Data == null)
        {
            return BadRequest(plansResponse);
        }

        var scheduleResponse = await _installmentScheduleService.GetAllAsync();
        if (!scheduleResponse.IsSuccess || scheduleResponse.Data == null)
        {
            return BadRequest(scheduleResponse);
        }

        var mapped = plansResponse.Data
            .Select(plan => MapPlan(plan, scheduleResponse.Data.Where(s => s.InstallmentPlanId == plan.Id)))
            .ToList();

        return Ok(ServiceResponse<IEnumerable<InstallmentPlanGetDto>>.Success(mapped));
    }

    [HttpGet("get-by-id/{uid}")]
    public async Task<IActionResult> GetById(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(ServiceResponse<InstallmentPlanGetDto>.Failure("Invalid encrypted id."));
        }

        var planResponse = await _installmentPlanService.GetByIdAsync(id);
        if (!planResponse.IsSuccess || planResponse.Data == null)
        {
            return NotFound(planResponse);
        }

        var scheduleResponse = await _installmentScheduleService.GetAllAsync();
        if (!scheduleResponse.IsSuccess || scheduleResponse.Data == null)
        {
            return BadRequest(scheduleResponse);
        }

        var dto = MapPlan(planResponse.Data, scheduleResponse.Data.Where(s => s.InstallmentPlanId == id));
        return Ok(ServiceResponse<InstallmentPlanGetDto>.Success(dto));
    }

    [HttpGet("get-schedules/{installmentPlanUid}")]
    public async Task<IActionResult> GetSchedules(string installmentPlanUid)
    {
        var planId = CipherHelper.DecryptId(installmentPlanUid);
        if (planId <= 0)
        {
            return BadRequest(ServiceResponse<IEnumerable<InstallmentScheduleGetDto>>.Failure("Invalid encrypted id."));
        }

        var response = await _installmentScheduleService.GetAllAsync();
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = response.Data
            .Where(s => s.InstallmentPlanId == planId)
            .Select(MapSchedule)
            .ToList();

        return Ok(ServiceResponse<IEnumerable<InstallmentScheduleGetDto>>.Success(mapped));
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateInstallmentTransactionRequestDto request)
    {
        var response = await _transactionFactory.CreateInstallmentAsync(request);
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateInstallmentTransactionRequestDto request)
    {
        var response = await _transactionFactory.UpdateInstallmentAsync(request);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpDelete("delete/{uid}")]
    public async Task<IActionResult> Delete(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(ServiceResponse.Failure("Invalid encrypted id."));
        }

        var response = await _installmentPlanService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return NoContent();
    }

    private static InstallmentPlanGetDto MapPlan(InstallmentPlan plan, IEnumerable<InstallmentSchedule> schedules)
    {
        return new InstallmentPlanGetDto
        {
            Uid = CipherHelper.EncryptId(plan.Id),
            UserHouseholdUid = CipherHelper.EncryptId(plan.UserHouseholdId),
            FinancialAccountUid = CipherHelper.EncryptId(plan.FinancialAccountId),
            CategoryUid = CipherHelper.EncryptId(plan.CategoryId),
            PurchaseDate = plan.PurchaseDate,
            TotalAmount = plan.TotalAmount,
            InstallmentCount = plan.InstallmentCount,
            FirstDueDate = plan.FirstDueDate,
            MerchantName = plan.MerchantName,
            Description = plan.Description,
            Schedules = schedules.Select(MapSchedule).ToList()
        };
    }

    private static InstallmentScheduleGetDto MapSchedule(InstallmentSchedule schedule)
    {
        return new InstallmentScheduleGetDto
        {
            Uid = CipherHelper.EncryptId(schedule.Id),
            InstallmentPlanUid = CipherHelper.EncryptId(schedule.InstallmentPlanId),
            InstallmentNo = schedule.InstallmentNo,
            DueDate = schedule.DueDate,
            Amount = schedule.Amount,
            EntryState = (int)schedule.EntryState,
            StatusType = (int)schedule.StatusType,
            ActualTransactionUid = schedule.ActualTransactionId.HasValue ? CipherHelper.EncryptId(schedule.ActualTransactionId.Value) : null
        };
    }
}