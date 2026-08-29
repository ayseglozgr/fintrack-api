using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.LedgerTransaction;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LedgerTransactionController : ControllerBase
{
    private readonly ITransactionFactory _transactionFactory;
    private readonly ILedgerTransactionService _ledgerTransactionService;

    public LedgerTransactionController(ITransactionFactory transactionFactory, ILedgerTransactionService ledgerTransactionService)
    {
        _transactionFactory = transactionFactory;
        _ledgerTransactionService = ledgerTransactionService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _ledgerTransactionService.GetAllAsync();
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = response.Data.Select(Map);
        return Ok(ServiceResponse<IEnumerable<LedgerTransactionGetDto>>.Success(mapped));
    }

    [HttpGet("get-by-id/{uid}")]
    public async Task<IActionResult> GetById(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(ServiceResponse<LedgerTransactionGetDto>.Failure("Invalid encrypted id."));
        }

        var response = await _ledgerTransactionService.GetByIdAsync(id);
        if (!response.IsSuccess || response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(ServiceResponse<LedgerTransactionGetDto>.Success(Map(response.Data)));
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateLedgerTransactionRequestDto request)
    {
        var response = await _transactionFactory.CreateIncomeExpenseAsync(request);
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetById), new { uid = response.Data.Uid }, response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateLedgerTransactionRequestDto request)
    {
        var response = await _transactionFactory.UpdateIncomeExpenseAsync(request);
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

        var response = await _ledgerTransactionService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return NoContent();
    }

    private static LedgerTransactionGetDto Map(LedgerTransaction entity)
    {
        return new LedgerTransactionGetDto
        {
            Uid = CipherHelper.EncryptId(entity.Id),
            UserHouseholdUid = CipherHelper.EncryptId(entity.UserHouseholdId),
            FinancialAccountUid = entity.FinancialAccountId.HasValue ? CipherHelper.EncryptId(entity.FinancialAccountId.Value) : null,
            CategoryUid = CipherHelper.EncryptId(entity.CategoryId),
            TransactionDate = entity.TransactionDate,
            Amount = entity.Amount,
            TransactionType = (int)entity.TransactionType,
            PaymentChannel = (int)entity.PaymentChannel,
            EntryState = (int)entity.EntryState,
            ExpenseKind = (int)entity.ExpenseKind,
            IsBudgetNeutral = entity.IsBudgetNeutral,
            Description = entity.Description
        };
    }
}