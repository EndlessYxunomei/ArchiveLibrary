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

    public async Task<Result<OriginalDetailDto>> GetOriginalDetailAsync(int id)
    {
        var res = await originalRepo.GetOriginalAsync(id);
        return res.IsSuccess ? Result<OriginalDetailDto>.Success((OriginalDetailDto)res.Data) : Result<OriginalDetailDto>.Fail(res.ErrorCode);
    }
    public async Task<Result<List<OriginalListDto>>> GetOriginalListAsync()
    {
        var originalList = await originalRepo.GetOriginalList();
        if (originalList.IsSuccess)
        {
            List<OriginalListDto> list = [];
            foreach (var original in originalList.Data)
            {
                list.Add((OriginalListDto)original);
            }
            return Result<List<OriginalListDto>>.Success(list);
        }
        return Result<List<OriginalListDto>>.Fail("Не удалось загрузить список оригиналов");
    }
    public async Task<Result<int>> UpsertOriginal(OriginalDetailDto originalDetailDto)
    {
        //Original newOriginal = new() 
        //{
        //    Id = originalDetailDto.Id,
        //    InventoryNumber = originalDetailDto.InventoryNumber,
        //    Name = originalDetailDto.Name,
        //    Caption = originalDetailDto.Caption,
        //    PageFormat = originalDetailDto.PageFormat,
        //    PageCount = originalDetailDto.PageCount,
        //    Notes = originalDetailDto.Notes,
        //    CompanyId = originalDetailDto.Company?.Id,
        //    DocumentId = originalDetailDto.Document?.Id,
        //    PersonId = originalDetailDto.Person?.Id,

        //    //CreatedDate = DateTime.Now
        //    //ПРИДУМАТЬ ЧТО ДЕЛАТЬ СО СПИСКАМИ КОПИЙ, КОРРЕКЦИ И ПРИМЕНИМОСТИ
        //};

        return await originalRepo.UpsertOriginal((Original)originalDetailDto);
    }
    public async Task<Result<Nothing>> DeleteOriginal(int id) => await originalRepo.DeleteOriginal(id);
    public async Task<Result<int>> GetLastInventoryNumber() => await originalRepo.GetLastInventoryNumberAsync();
    public async Task<Result<Nothing>> CheckInventoryNumber(int inventorynumber) => await originalRepo.CheckInventoryNumberAsync(inventorynumber);
    public async Task<Result<OriginalListDto>> GetOriginalAsync(int id)
    {
        var res = await originalRepo.GetOriginalAsync(id);
        return res.IsSuccess ? Result<OriginalListDto>.Success((OriginalListDto)res.Data) : Result<OriginalListDto>.Fail(res.ErrorCode);
    }

    public async Task<Result<List<OriginalListDto>>> GetOriginalsByCompany(int companyId)
    {
        var originalList = await originalRepo.GetOriginalsByCompany(companyId);
        if (originalList.IsSuccess)
        {
            List<OriginalListDto> list = [];
            list.AddRange(originalList.Data.Select(original => (OriginalListDto)original));
            return Result<List<OriginalListDto>>.Success(list);
        }
        return Result<List<OriginalListDto>>.Fail("Не удалось загрузить список компаний");
    }

    public async Task<Result<Nothing>> UpdateOriginalsApplicabilities(int id, List<ApplicabilityDto> applicabilityDtos)
    {
        List<int> applicabilityIds = [];
        foreach (ApplicabilityDto app in applicabilityDtos)
        {
            applicabilityIds.Add(app.Id);
        }
        return await originalRepo.UpdateOriginalApplicabilities(id, applicabilityIds);

    }

    public async Task<Result<List<OriginalListDto>>> GetOriginalsByApplicability(int applicabilityId)
    {
        var originalList = await originalRepo.GetOriginalsByApplicability(applicabilityId);
        if (originalList.IsSuccess)
        {
            List<OriginalListDto> list = [];
            list.AddRange(originalList.Data.Select(original => (OriginalListDto)original));
            return Result<List<OriginalListDto>>.Success(list);
        }
        return Result<List<OriginalListDto>>.Fail("Не удалось загрузить список оригиналов");
    }
}
