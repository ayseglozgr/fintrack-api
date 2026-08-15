using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Application.Interfaces.Services;
using FinTrack.Application.Common.Helpers;
using FinTrack.Application.Common.Models;
using FinTrack.Application.DTOs.FinancialAccount;
using FinTrack.Domain.Entities;
using static FinTrack.Domain.Const.Enum;

namespace FinTrack.Infrastructure.Services;

public class FinancialAccountService : IFinancialAccountService
{
    private readonly IFinancialAccountRepository _financialAccountRepository;

    public FinancialAccountService(IFinancialAccountRepository financialAccountRepository)
    {
        _financialAccountRepository = financialAccountRepository;
    }

    public async Task<ServiceResponse<IEnumerable<FinancialAccountGetDto>>> GetAllAsync(FinancialAccountType? type = null)
    {
        if (type.HasValue)
        {
            var filteredAccounts = await _financialAccountRepository.FindAsync(f => f.Type == type.Value);
            var filteredDtos = filteredAccounts.Select(MapToGetDto);
            return ServiceResponse<IEnumerable<FinancialAccountGetDto>>.Success(filteredDtos);
        }

        var allAccounts = await _financialAccountRepository.GetAllAsync();
        var allDtos = allAccounts.Select(MapToGetDto);
        return ServiceResponse<IEnumerable<FinancialAccountGetDto>>.Success(allDtos);
    }

    public async Task<ServiceResponse<FinancialAccountGetDto>> GetByIdAsync(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Invalid encrypted id.");
        }

        var account = await _financialAccountRepository.GetByIdAsync(id);
        if (account == null)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Financial account not found.");
        }

        return ServiceResponse<FinancialAccountGetDto>.Success(MapToGetDto(account));
    }

    public async Task<ServiceResponse<FinancialAccountGetDto>> CreateAsync(CreateFinancialAccountRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Financial account payload is required.");
        }

        var financialAccount = new FinancialAccount
        {
            UserId = request.UserId,
            Type = request.Type,
            ProviderName = request.ProviderName,
            Alias = request.Alias,
            Last4 = request.Last4,
            IsActive = request.IsActive
        };

        await _financialAccountRepository.AddAsync(financialAccount);
        return ServiceResponse<FinancialAccountGetDto>.Success(
            MapToGetDto(financialAccount),
            "Financial account created successfully.");
    }

    public async Task<ServiceResponse<FinancialAccountGetDto>> UpdateAsync(UpdateFinancialAccountRequestDto request)
    {
        if (request == null)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Financial account payload is required.");
        }

        var id = CipherHelper.DecryptId(request.Uid);
        if (id <= 0)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Invalid encrypted id.");
        }

        var existingFinancialAccount = await _financialAccountRepository.GetByIdAsync(id);
        if (existingFinancialAccount == null)
        {
            return ServiceResponse<FinancialAccountGetDto>.Failure("Financial account not found.");
        }

        //existingFinancialAccount.UserId = request.UserId; hane halkından baska biri diğerinin kartında güncelleme yapması gerektiğinde güncelleyen kişiye geçmemesi için yapıldı.
        existingFinancialAccount.Type = request.Type;
        existingFinancialAccount.ProviderName = request.ProviderName;
        existingFinancialAccount.Alias = request.Alias;
        existingFinancialAccount.Last4 = request.Last4;
        existingFinancialAccount.IsActive = request.IsActive;

        await _financialAccountRepository.UpdateAsync(existingFinancialAccount);
        return ServiceResponse<FinancialAccountGetDto>.Success(
            MapToGetDto(existingFinancialAccount),
            "Financial account updated successfully.");
    }

    public async Task<ServiceResponse> DeleteAsync(string uid)
    {
        var id = CipherHelper.DecryptId(uid);
        if (id <= 0)
        {
            return ServiceResponse.Failure("Invalid encrypted id.");
        }

        var existingFinancialAccount = await _financialAccountRepository.GetByIdAsync(id);
        if (existingFinancialAccount == null)
        {
            return ServiceResponse.Failure("Financial account not found.");
        }

        await _financialAccountRepository.DeleteAsync(existingFinancialAccount);
        return ServiceResponse.Success("Financial account deleted successfully.");
    }

    private static FinancialAccountGetDto MapToGetDto(FinancialAccount financialAccount)
    {
        return new FinancialAccountGetDto
        {
            Uid = CipherHelper.EncryptId(financialAccount.Id),
            UserId = financialAccount.UserId,
            Type = (int)financialAccount.Type,
            TypeDescriptionTr = financialAccount.Type.GetDescription(),
            ProviderName = financialAccount.ProviderName,
            Alias = financialAccount.Alias,
            Last4 = financialAccount.Last4,
            IsActive = financialAccount.IsActive
        };
    }
}
