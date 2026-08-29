using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.ProxyTransaction;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProxyTransactionsController : ControllerBase
{
    private readonly ITransactionFactory _transactionFactory;
    private readonly IProxyCaseService _proxyCaseService;
    private readonly IProxySettlementService _proxySettlementService;

    public ProxyTransactionsController(
        ITransactionFactory transactionFactory,
        IProxyCaseService proxyCaseService,
        IProxySettlementService proxySettlementService)
    {
        _transactionFactory = transactionFactory;
        _proxyCaseService = proxyCaseService;
        _proxySettlementService = proxySettlementService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _proxyCaseService.GetAllAsync();
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = response.Data.Select(MapCase);
        return Ok(ServiceResponse<IEnumerable<ProxyCaseGetDto>>.Success(mapped));
    }

    [HttpGet("get-by-id/{uid}")]
    public async Task<IActionResult> GetById(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return BadRequest(ServiceResponse<ProxyCaseGetDto>.Failure("Invalid encrypted id."));
        }

        var response = await _proxyCaseService.GetByIdAsync(id);
        if (!response.IsSuccess || response.Data == null)
        {
            return NotFound(response);
        }

        return Ok(ServiceResponse<ProxyCaseGetDto>.Success(MapCase(response.Data)));
    }

    [HttpGet("get-settlements/{proxyCaseUid}")]
    public async Task<IActionResult> GetSettlements(string proxyCaseUid)
    {
        var proxyCaseId = CipherHelper.DecryptId(proxyCaseUid);
        if (proxyCaseId <= 0)
        {
            return BadRequest(ServiceResponse<IEnumerable<ProxySettlementGetDto>>.Failure("Invalid encrypted id."));
        }

        var response = await _proxySettlementService.GetAllAsync();
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        var mapped = response.Data
            .Where(s => s.ProxyCaseId == proxyCaseId)
            .Select(MapSettlement)
            .ToList();

        return Ok(ServiceResponse<IEnumerable<ProxySettlementGetDto>>.Success(mapped));
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartProxyTransactionRequestDto request)
    {
        var response = await _transactionFactory.StartProxyAsync(request);
        if (!response.IsSuccess || response.Data == null)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("close")]
    public async Task<IActionResult> Close([FromBody] CloseProxyTransactionRequestDto request)
    {
        var response = await _transactionFactory.CloseProxyAsync(request);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
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

        var response = await _proxyCaseService.DeleteAsync(id);
        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return NoContent();
    }

    private static ProxyCaseGetDto MapCase(ProxyCase entity)
    {
        return new ProxyCaseGetDto
        {
            Uid = CipherHelper.EncryptId(entity.Id),
            UserHouseholdUid = CipherHelper.EncryptId(entity.UserHouseholdId),
            FinancialAccountUid = CipherHelper.EncryptId(entity.FinancialAccountId),
            ExternalPartyName = entity.ExternalPartyName,
            SpentDate = entity.SpentDate,
            TotalAdvancedAmount = entity.TotalAdvancedAmount,
            Status = (int)entity.Status,
            ClosedDate = entity.ClosedDate,
            Description = entity.Description
        };
    }

    private static ProxySettlementGetDto MapSettlement(ProxySettlement entity)
    {
        return new ProxySettlementGetDto
        {
            Uid = CipherHelper.EncryptId(entity.Id),
            ProxyCaseUid = CipherHelper.EncryptId(entity.ProxyCaseId),
            SettlementDate = entity.SettlementDate,
            Amount = entity.Amount,
            PaymentChannel = (int)entity.PaymentChannel,
            EntryState = (int)entity.EntryState,
            ReceivedFinancialAccountUid = entity.ReceivedFinancialAccountId.HasValue ? CipherHelper.EncryptId(entity.ReceivedFinancialAccountId.Value) : null,
            SettlementTransactionUid = entity.SettlementTransactionId.HasValue ? CipherHelper.EncryptId(entity.SettlementTransactionId.Value) : null,
            Note = entity.Note
        };
    }
}