using FinTrack.Application.Common.Models;
using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Services;

public class LedgerTransactionService : ILedgerTransactionService
{
    private readonly ILedgerTransactionRepository _ledgerTransactionRepository;

    public LedgerTransactionService(ILedgerTransactionRepository ledgerTransactionRepository)
    {
        _ledgerTransactionRepository = ledgerTransactionRepository;
    }

    public async Task<ServiceResponse<IEnumerable<LedgerTransaction>>> GetAllAsync()
    {
        var transactions = await _ledgerTransactionRepository.GetAllAsync();
        return ServiceResponse<IEnumerable<LedgerTransaction>>.Success(transactions);
    }

    public async Task<ServiceResponse<LedgerTransaction>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse<LedgerTransaction>.Failure("Invalid transaction id.");
        }

        var transaction = await _ledgerTransactionRepository.GetByIdAsync(id);
        if (transaction == null)
        {
            return ServiceResponse<LedgerTransaction>.Failure("Transaction not found.");
        }

        return ServiceResponse<LedgerTransaction>.Success(transaction);
    }

    public async Task<ServiceResponse<LedgerTransaction>> CreateAsync(LedgerTransaction transaction)
    {
        if (transaction == null)
        {
            return ServiceResponse<LedgerTransaction>.Failure("Transaction payload is required.");
        }

        await _ledgerTransactionRepository.AddAsync(transaction);
        return ServiceResponse<LedgerTransaction>.Success(transaction, "Transaction created successfully.");
    }

    public async Task<ServiceResponse<LedgerTransaction>> UpdateAsync(LedgerTransaction transaction)
    {
        if (transaction == null || transaction.Id <= 0)
        {
            return ServiceResponse<LedgerTransaction>.Failure("Valid transaction payload is required.");
        }

        var existing = await _ledgerTransactionRepository.GetByIdAsync(transaction.Id);
        if (existing == null)
        {
            return ServiceResponse<LedgerTransaction>.Failure("Transaction not found.");
        }

        existing.UserHouseholdId = transaction.UserHouseholdId;
        existing.FinancialAccountId = transaction.FinancialAccountId;
        existing.CategoryId = transaction.CategoryId;
        existing.TransactionDate = transaction.TransactionDate;
        existing.Amount = transaction.Amount;
        existing.TransactionType = transaction.TransactionType;
        existing.PaymentChannel = transaction.PaymentChannel;
        existing.EntryState = transaction.EntryState;
        existing.ExpenseKind = transaction.ExpenseKind;
        existing.IsBudgetNeutral = transaction.IsBudgetNeutral;
        existing.Description = transaction.Description;

        await _ledgerTransactionRepository.UpdateAsync(existing);
        return ServiceResponse<LedgerTransaction>.Success(existing, "Transaction updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid transaction id.");
        }

        var existing = await _ledgerTransactionRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return ServiceResponse.Failure("Transaction not found.");
        }

        await _ledgerTransactionRepository.DeleteAsync(existing);
        return ServiceResponse.Success("Transaction deleted successfully.");
    }
}