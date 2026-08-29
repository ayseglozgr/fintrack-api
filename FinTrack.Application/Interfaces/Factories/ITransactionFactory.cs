using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Installment;
using FinTrack.Application.DTOs.LedgerTransaction;
using FinTrack.Application.DTOs.ProxyTransaction;

namespace FinTrack.Application.Interfaces.Factories;

public interface ITransactionFactory
{
    Task<ServiceResponse<LedgerTransactionGetDto>> CreateIncomeExpenseAsync(CreateLedgerTransactionRequestDto request);
    Task<ServiceResponse<LedgerTransactionGetDto>> UpdateIncomeExpenseAsync(UpdateLedgerTransactionRequestDto request);

    Task<ServiceResponse<InstallmentOrchestrationResponseDto>> CreateInstallmentAsync(CreateInstallmentTransactionRequestDto request);
    Task<ServiceResponse<InstallmentOrchestrationResponseDto>> UpdateInstallmentAsync(UpdateInstallmentTransactionRequestDto request);

    Task<ServiceResponse<ProxyOrchestrationResponseDto>> StartProxyAsync(StartProxyTransactionRequestDto request);
    Task<ServiceResponse<ProxyOrchestrationResponseDto>> CloseProxyAsync(CloseProxyTransactionRequestDto request);
}