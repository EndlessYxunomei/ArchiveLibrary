using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IOriginalRepo
{
    Task<Result<List<Original>>> GetOriginalList();
    Task<Result<Original>> GetOriginalAsync(int id);
    Task<Result<List<Original>>> GetOriginalsByDocument(int docunentId);
    Task<Result<List<Original>>> GetOriginalsByCompany(int companyId);
    Task<Result<List<Original>>> GetOriginalsByApplicability(int applicabilityId);
    Task<Result<int>> GetLastInventoryNumberAsync();
    Task<Result<Nothing>> CheckInventoryNumberAsync(int inventoryNumber);
    Task<Result<int>> UpsertOriginal(Original original);
    //Task UpsertOriginals(List<Original> originals);
    Task<Result<Nothing>> DeleteOriginal(int id);
    //Task DeleteOriginals(List<int> originalIds);
    Task<Result<Nothing>> UpdateOriginalApplicabilities(int id, List<int> applicabilityId);
}
