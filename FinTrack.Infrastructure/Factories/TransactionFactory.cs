using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.Installment;
using FinTrack.Application.DTOs.LedgerTransaction;
using FinTrack.Application.DTOs.ProxyTransaction;
using FinTrack.Application.Interfaces.Factories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Infrastructure.Factories;

public class TransactionFactory : ITransactionFactory
{
    private readonly ILedgerTransactionService _ledgerTransactionService;
    private readonly IInstallmentPlanService _installmentPlanService;
    private readonly IInstallmentScheduleService _installmentScheduleService;
    private readonly IProxyCaseService _proxyCaseService;
    private readonly IProxySettlementService _proxySettlementService;

    public TransactionFactory(
        ILedgerTransactionService ledgerTransactionService,
        IInstallmentPlanService installmentPlanService,
        IInstallmentScheduleService installmentScheduleService,
        IProxyCaseService proxyCaseService,
        IProxySettlementService proxySettlementService)
    {
        _ledgerTransactionService = ledgerTransactionService;
        _installmentPlanService = installmentPlanService;
        _installmentScheduleService = installmentScheduleService;
        _proxyCaseService = proxyCaseService;
        _proxySettlementService = proxySettlementService;
    }

    public async Task<ServiceResponse<LedgerTransactionGetDto>> CreateIncomeExpenseAsync(CreateLedgerTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure("Transaction payload is required.");
        }

        var userHouseholdId = CipherHelper.DecryptId(request.UserHouseholdUid);
        var categoryId = CipherHelper.DecryptId(request.CategoryUid);
        var financialAccountId = string.IsNullOrWhiteSpace(request.FinancialAccountUid)
            ? (int?)null
            : CipherHelper.DecryptId(request.FinancialAccountUid);

        if (userHouseholdId <= 0 || categoryId <= 0 || (financialAccountId.HasValue && financialAccountId <= 0))
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure("Invalid uid value in request.");
        }

        var entity = new LedgerTransaction
        {
            UserHouseholdId = userHouseholdId,
            CategoryId = categoryId,
            FinancialAccountId = financialAccountId,
            TransactionDate = request.TransactionDate,
            Amount = request.Amount,
            TransactionType = request.TransactionType,
            PaymentChannel = request.PaymentChannel,
            EntryState = request.EntryState,
            ExpenseKind = request.ExpenseKind,
            IsBudgetNeutral = request.IsBudgetNeutral,
            Description = request.Description
        };

        var response = await _ledgerTransactionService.CreateAsync(entity);
        if (!response.IsSuccess || response.Data == null)
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure(response.Message, response.Errors);
        }

        return ServiceResponse<LedgerTransactionGetDto>.Success(MapLedgerTransaction(response.Data), response.Message);
    }

    public async Task<ServiceResponse<LedgerTransactionGetDto>> UpdateIncomeExpenseAsync(UpdateLedgerTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure("Transaction payload is required.");
        }

        var id = CipherHelper.DecryptId(request.Uid);
        var userHouseholdId = CipherHelper.DecryptId(request.UserHouseholdUid);
        var categoryId = CipherHelper.DecryptId(request.CategoryUid);
        var financialAccountId = string.IsNullOrWhiteSpace(request.FinancialAccountUid)
            ? (int?)null
            : CipherHelper.DecryptId(request.FinancialAccountUid);

        if (id <= 0 || userHouseholdId <= 0 || categoryId <= 0 || (financialAccountId.HasValue && financialAccountId <= 0))
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure("Invalid uid value in request.");
        }

        var entity = new LedgerTransaction
        {
            Id = id,
            UserHouseholdId = userHouseholdId,
            CategoryId = categoryId,
            FinancialAccountId = financialAccountId,
            TransactionDate = request.TransactionDate,
            Amount = request.Amount,
            TransactionType = request.TransactionType,
            PaymentChannel = request.PaymentChannel,
            EntryState = request.EntryState,
            ExpenseKind = request.ExpenseKind,
            IsBudgetNeutral = request.IsBudgetNeutral,
            Description = request.Description
        };

        var response = await _ledgerTransactionService.UpdateAsync(entity);
        if (!response.IsSuccess || response.Data == null)
        {
            return ServiceResponse<LedgerTransactionGetDto>.Failure(response.Message, response.Errors);
        }

        return ServiceResponse<LedgerTransactionGetDto>.Success(MapLedgerTransaction(response.Data), response.Message);
    }

    public async Task<ServiceResponse<InstallmentOrchestrationResponseDto>> CreateInstallmentAsync(CreateInstallmentTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure("Installment payload is required.");
        }

        var userHouseholdId = CipherHelper.DecryptId(request.UserHouseholdUid);
        var categoryId = CipherHelper.DecryptId(request.CategoryUid);
        var financialAccountId = CipherHelper.DecryptId(request.FinancialAccountUid);

        if (userHouseholdId <= 0 || categoryId <= 0 || financialAccountId <= 0)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure("Invalid uid value in request.");
        }

        var plan = new InstallmentPlan
        {
            UserHouseholdId = userHouseholdId,
            FinancialAccountId = financialAccountId,
            CategoryId = categoryId,
            PurchaseDate = request.PurchaseDate,
            TotalAmount = request.TotalAmount,
            InstallmentCount = request.InstallmentCount,
            FirstDueDate = request.FirstDueDate,
            MerchantName = request.MerchantName,
            Description = request.Description
        };

        var planResponse = await _installmentPlanService.CreateAsync(plan);
        if (!planResponse.IsSuccess || planResponse.Data == null)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(planResponse.Message, planResponse.Errors);
        }

        var installmentAmounts = BuildInstallmentAmounts(request.TotalAmount, request.InstallmentCount);
        var scheduleUids = new List<string>();

        for (short i = 1; i <= request.InstallmentCount; i++)
        {
            var schedule = new InstallmentSchedule
            {
                InstallmentPlanId = planResponse.Data.Id,
                InstallmentNo = i,
                DueDate = request.FirstDueDate.AddMonths(i - 1),
                Amount = installmentAmounts[i - 1],
                EntryState = request.EntryState,
                StatusType = request.EntryState == EntryState.Actual ? StatusType.Actual : StatusType.DraftProjected
            };

            var scheduleResponse = await _installmentScheduleService.CreateAsync(schedule);
            if (!scheduleResponse.IsSuccess || scheduleResponse.Data == null)
            {
                return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(scheduleResponse.Message, scheduleResponse.Errors);
            }

            scheduleUids.Add(CipherHelper.EncryptId(scheduleResponse.Data.Id));
        }

        return ServiceResponse<InstallmentOrchestrationResponseDto>.Success(new InstallmentOrchestrationResponseDto
        {
            InstallmentPlanUid = CipherHelper.EncryptId(planResponse.Data.Id),
            InstallmentScheduleUids = scheduleUids
        }, "Installment plan and schedules created successfully.");
    }

    public async Task<ServiceResponse<InstallmentOrchestrationResponseDto>> UpdateInstallmentAsync(UpdateInstallmentTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure("Installment payload is required.");
        }

        var planId = CipherHelper.DecryptId(request.InstallmentPlanUid);
        var userHouseholdId = CipherHelper.DecryptId(request.UserHouseholdUid);
        var categoryId = CipherHelper.DecryptId(request.CategoryUid);
        var financialAccountId = CipherHelper.DecryptId(request.FinancialAccountUid);

        if (planId <= 0 || userHouseholdId <= 0 || categoryId <= 0 || financialAccountId <= 0)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure("Invalid uid value in request.");
        }

        var planUpdate = new InstallmentPlan
        {
            Id = planId,
            UserHouseholdId = userHouseholdId,
            FinancialAccountId = financialAccountId,
            CategoryId = categoryId,
            PurchaseDate = request.PurchaseDate,
            TotalAmount = request.TotalAmount,
            InstallmentCount = request.InstallmentCount,
            FirstDueDate = request.FirstDueDate,
            MerchantName = request.MerchantName,
            Description = request.Description
        };

        var planUpdateResponse = await _installmentPlanService.UpdateAsync(planUpdate);
        if (!planUpdateResponse.IsSuccess || planUpdateResponse.Data == null)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(planUpdateResponse.Message, planUpdateResponse.Errors);
        }

        var schedulesResponse = await _installmentScheduleService.GetAllAsync();
        if (!schedulesResponse.IsSuccess || schedulesResponse.Data == null)
        {
            return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(schedulesResponse.Message, schedulesResponse.Errors);
        }

        var existingSchedules = schedulesResponse.Data.Where(s => s.InstallmentPlanId == planId).ToList();
        foreach (var schedule in existingSchedules)
        {
            var deleteResponse = await _installmentScheduleService.DeleteAsync(schedule.Id);
            if (!deleteResponse.IsSuccess)
            {
                return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(deleteResponse.Message, deleteResponse.Errors);
            }
        }

        var installmentAmounts = BuildInstallmentAmounts(request.TotalAmount, request.InstallmentCount);
        var newScheduleUids = new List<string>();

        for (short i = 1; i <= request.InstallmentCount; i++)
        {
            var schedule = new InstallmentSchedule
            {
                InstallmentPlanId = planId,
                InstallmentNo = i,
                DueDate = request.FirstDueDate.AddMonths(i - 1),
                Amount = installmentAmounts[i - 1],
                EntryState = request.EntryState,
                StatusType = request.EntryState == EntryState.Actual ? StatusType.Actual : StatusType.DraftProjected
            };

            var createScheduleResponse = await _installmentScheduleService.CreateAsync(schedule);
            if (!createScheduleResponse.IsSuccess || createScheduleResponse.Data == null)
            {
                return ServiceResponse<InstallmentOrchestrationResponseDto>.Failure(createScheduleResponse.Message, createScheduleResponse.Errors);
            }

            newScheduleUids.Add(CipherHelper.EncryptId(createScheduleResponse.Data.Id));
        }

        return ServiceResponse<InstallmentOrchestrationResponseDto>.Success(new InstallmentOrchestrationResponseDto
        {
            InstallmentPlanUid = request.InstallmentPlanUid,
            InstallmentScheduleUids = newScheduleUids
        }, "Installment plan and schedules updated successfully.");
    }

    public async Task<ServiceResponse<ProxyOrchestrationResponseDto>> StartProxyAsync(StartProxyTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure("Proxy payload is required.");
        }

        var userHouseholdId = CipherHelper.DecryptId(request.UserHouseholdUid);
        var financialAccountId = CipherHelper.DecryptId(request.FinancialAccountUid);

        if (userHouseholdId <= 0 || financialAccountId <= 0)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure("Invalid uid value in request.");
        }

        var proxyCase = new ProxyCase
        {
            UserHouseholdId = userHouseholdId,
            FinancialAccountId = financialAccountId,
            ExternalPartyName = request.ExternalPartyName,
            SpentDate = request.SpentDate,
            TotalAdvancedAmount = request.TotalAdvancedAmount,
            Description = request.Description,
            Status = SettlementStatus.Open
        };

        var response = await _proxyCaseService.CreateAsync(proxyCase);
        if (!response.IsSuccess || response.Data == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure(response.Message, response.Errors);
        }

        return ServiceResponse<ProxyOrchestrationResponseDto>.Success(new ProxyOrchestrationResponseDto
        {
            ProxyCaseUid = CipherHelper.EncryptId(response.Data.Id)
        }, "Proxy case created successfully.");
    }

    public async Task<ServiceResponse<ProxyOrchestrationResponseDto>> CloseProxyAsync(CloseProxyTransactionRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure("Proxy close payload is required.");
        }

        var proxyCaseId = CipherHelper.DecryptId(request.ProxyCaseUid);
        var receivedFinancialAccountId = string.IsNullOrWhiteSpace(request.ReceivedFinancialAccountUid)
            ? (int?)null
            : CipherHelper.DecryptId(request.ReceivedFinancialAccountUid);

        if (proxyCaseId <= 0 || (receivedFinancialAccountId.HasValue && receivedFinancialAccountId <= 0))
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure("Invalid uid value in request.");
        }

        var proxyCaseResponse = await _proxyCaseService.GetByIdAsync(proxyCaseId);
        if (!proxyCaseResponse.IsSuccess || proxyCaseResponse.Data == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure(proxyCaseResponse.Message, proxyCaseResponse.Errors);
        }

        var settlement = new ProxySettlement
        {
            ProxyCaseId = proxyCaseId,
            SettlementDate = request.SettlementDate,
            Amount = request.Amount,
            PaymentChannel = request.PaymentChannel,
            EntryState = request.EntryState,
            ReceivedFinancialAccountId = receivedFinancialAccountId,
            Note = request.Note
        };

        var settlementResponse = await _proxySettlementService.CreateAsync(settlement);
        if (!settlementResponse.IsSuccess || settlementResponse.Data == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure(settlementResponse.Message, settlementResponse.Errors);
        }

        var allSettlementsResponse = await _proxySettlementService.GetAllAsync();
        if (!allSettlementsResponse.IsSuccess || allSettlementsResponse.Data == null)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure(allSettlementsResponse.Message, allSettlementsResponse.Errors);
        }

        var collectedTotal = allSettlementsResponse.Data
            .Where(s => s.ProxyCaseId == proxyCaseId)
            .Sum(s => s.Amount);

        var proxyCase = proxyCaseResponse.Data;
        if (collectedTotal >= proxyCase.TotalAdvancedAmount)
        {
            proxyCase.Status = SettlementStatus.Settled;
            proxyCase.ClosedDate = request.SettlementDate;
        }
        else
        {
            proxyCase.Status = SettlementStatus.PartiallySettled;
        }

        var proxyUpdateResponse = await _proxyCaseService.UpdateAsync(proxyCase);
        if (!proxyUpdateResponse.IsSuccess)
        {
            return ServiceResponse<ProxyOrchestrationResponseDto>.Failure(proxyUpdateResponse.Message, proxyUpdateResponse.Errors);
        }

        return ServiceResponse<ProxyOrchestrationResponseDto>.Success(new ProxyOrchestrationResponseDto
        {
            ProxyCaseUid = request.ProxyCaseUid,
            LatestSettlementUid = CipherHelper.EncryptId(settlementResponse.Data.Id)
        }, "Proxy case settlement recorded successfully.");
    }

    private static LedgerTransactionGetDto MapLedgerTransaction(LedgerTransaction entity)
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

    private static List<decimal> BuildInstallmentAmounts(decimal totalAmount, short installmentCount)
    {
        var baseAmount = Math.Floor((totalAmount / installmentCount) * 100) / 100;
        var values = Enumerable.Repeat(baseAmount, installmentCount).ToList();
        var distributed = values.Sum();
        values[^1] += totalAmount - distributed;
        return values;
    }
}
