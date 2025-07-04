using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer;

public class OriginalService : IOriginalService
{
    private readonly IOriginalRepo originalRepo;
    public OriginalService(ArchiveDbContext context)
    {
        originalRepo = new OriginalRepo(context);
    }
    public OriginalService(IOriginalRepo originalRepo)
    {
        this.originalRepo = originalRepo;
    }

    //получение единичных объектов
    public async Task<Result<OriginalDetailDto>> GetOriginalDetailAsync(int id)
    {
        var res = await originalRepo.GetOriginalDetailAsync(id);
        return res.IsSuccess
            ? Result<OriginalDetailDto>.Success(res.Data)
            : Result<OriginalDetailDto>.Fail(res.ErrorCode, res.ErrorData, res.Exception);
    }
    public async Task<Result<OriginalListDto>> GetOriginalAsync(int id)
    {
        var res = await originalRepo.GetOriginalAsync(id);
        return res.IsSuccess
            ? Result<OriginalListDto>.Success(res.Data)
            : Result<OriginalListDto>.Fail(res.ErrorCode, res.ErrorData, res.Exception);
    }

    //получение списков
    public async Task<Result<List<OriginalListDto>>> GetOriginalListAsync()
    {
        var originalList = await originalRepo.GetOriginalList();
        return originalList.IsSuccess
            ? Result<List<OriginalListDto>>.Success(originalList.Data)
            : Result<List<OriginalListDto>>.Fail(originalList.ErrorCode, originalList.ErrorData, originalList.Exception);
    }
    public async Task<Result<List<OriginalListDto>>> GetOriginalsByCompany(int companyId)
    {
        var originalList = await originalRepo.GetOriginalsByCompany(companyId);
        return originalList.IsSuccess
            ? Result<List<OriginalListDto>>.Success(originalList.Data)
            : Result<List<OriginalListDto>>.Fail(originalList.ErrorCode, originalList.ErrorData, originalList.Exception);
    }
    public async Task<Result<List<OriginalListDto>>> GetOriginalsByApplicability(int applicabilityId)
    {
        var originalList = await originalRepo.GetOriginalsByApplicability(applicabilityId);
        return originalList.IsSuccess
            ? Result<List<OriginalListDto>>.Success(originalList.Data)
            : Result<List<OriginalListDto>>.Fail(originalList.ErrorCode, originalList.ErrorData, originalList.Exception);
    }

    //создание обновление и удаление
    public async Task<Result<OriginalListDto>> UpsertOriginal(OriginalDetailDto originalDetailDto)
    {
        var newOriginalId = await originalRepo.UpsertOriginal(originalDetailDto);
        if (newOriginalId.IsSuccess)
        {
            var newOriginal = await originalRepo.GetOriginalAsync(newOriginalId.Data);
            if (newOriginal.IsSuccess)
            {
                //await originalService.UpdateOriginalsApplicabilities(id, [.. ApplicabilityList]);
                return Result<OriginalListDto>.Success(newOriginal.Data);
            }
            return Result<OriginalListDto>.Fail(newOriginal.ErrorCode, newOriginal.ErrorData, newOriginal.Exception);
        }
        return Result<OriginalListDto>.Fail(newOriginalId.ErrorCode, newOriginalId.ErrorData, newOriginalId.Exception);
    }
    public async Task<Result<Nothing>> DeleteOriginal(int id) => await originalRepo.DeleteOriginal(id);
    public async Task<Result<Nothing>> UpdateOriginalsApplicabilities(int id, List<ApplicabilityDto> applicabilityDtos)
    {
        List<int> applicabilityIds = [];
        foreach (ApplicabilityDto app in applicabilityDtos)
        {
            applicabilityIds.Add(app.Id);
        }
        return await originalRepo.UpdateOriginalApplicabilities(id, applicabilityIds);

    }

    //работа с инвентарными номерами
    public async Task<Result<int>> GetLastInventoryNumber() => await originalRepo.GetLastInventoryNumberAsync();
    public async Task<Result<Nothing>> CheckInventoryNumber(int inventorynumber) => await originalRepo.CheckInventoryNumberAsync(inventorynumber);
}
