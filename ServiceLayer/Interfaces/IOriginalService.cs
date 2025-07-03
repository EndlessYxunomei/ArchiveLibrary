using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface IOriginalService
{
    Task<Result<List<OriginalListDto>>> GetOriginalListAsync();
    Task<Result<OriginalDetailDto>> GetOriginalDetailAsync(int id);
    Task<Result<OriginalListDto>> GetOriginalAsync(int id);
    Task<Result<int>> GetLastInventoryNumber();
    Task<Result<Nothing>> CheckInventoryNumber(int inventorynumber);
    Task<Result<OriginalListDto>> UpsertOriginal(OriginalDetailDto originalDetailDto);
    Task<Result<Nothing>> DeleteOriginal(int id);
    Task<Result<List<OriginalListDto>>> GetOriginalsByCompany(int companyId);
    Task<Result<List<OriginalListDto>>> GetOriginalsByApplicability(int applicabilityId);
    Task<Result<Nothing>> UpdateOriginalsApplicabilities(int id, List<ApplicabilityDto> applicabilityDtos);
}
