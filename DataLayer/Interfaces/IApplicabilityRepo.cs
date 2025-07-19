using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IApplicabilityRepo
{
    Task<Result<List<ApplicabilityDto>>> GetApplicabilityListAsync();
    Task<Result<List<ApplicabilityDto>>> GetApplicabilityListByOriginal(int originalId);
    Task<Result<List<ApplicabilityDto>>> GetFreeApplicabilityList(int originalId);

    Task<Result<Nothing>> CheckApplicability(string description);

    Task<Result<ApplicabilityDto>> GetApplicabilityAsync(int id);

    Task<Result<ApplicabilityDto>> UpsertApplicability(ApplicabilityDto applicability);
    Task<Result<Nothing>> DeleteApplicability(int id);

    Task<Result<Nothing>> DeleteOriginalFromApplicability(int id, int originalId);
    Task<Result<Nothing>> AddOriginalToApplicability(int id, int originalId);
}
