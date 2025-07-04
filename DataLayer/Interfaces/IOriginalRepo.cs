using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IOriginalRepo
{
    Task<Result<List<OriginalListDto>>> GetOriginalList();
    Task<Result<OriginalDetailDto>> GetOriginalDetailAsync(int id);
    Task<Result<OriginalListDto>> GetOriginalAsync(int id);
    Task<Result<List<OriginalListDto>>> GetOriginalsByDocument(int docunentId);
    Task<Result<List<OriginalListDto>>> GetOriginalsByCompany(int companyId);
    Task<Result<List<OriginalListDto>>> GetOriginalsByApplicability(int applicabilityId);
    Task<Result<int>> GetLastInventoryNumberAsync();
    Task<Result<Nothing>> CheckInventoryNumberAsync(int inventoryNumber);
    Task<Result<int>> UpsertOriginal(OriginalDetailDto original);
    //Task UpsertOriginals(List<Original> originals);
    Task<Result<Nothing>> DeleteOriginal(int id);
    //Task DeleteOriginals(List<int> originalIds);
    Task<Result<Nothing>> UpdateOriginalApplicabilities(int id, List<int> applicabilityId);
}
