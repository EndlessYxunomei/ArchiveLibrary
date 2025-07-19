using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ApplicabilityService : IApplicabilityService
    {
        private readonly IApplicabilityRepo applicabilityRepo;

        public ApplicabilityService(IApplicabilityRepo applicabilityRepo)
        {
            this.applicabilityRepo = applicabilityRepo;
        }
        public ApplicabilityService(ArchiveDbContext context)
        {
            applicabilityRepo = new ApplicabilityRepo(context);
        }

        public async Task<Result<Nothing>> AddOriginalToApplicability(int id, int originalId) => await applicabilityRepo.AddOriginalToApplicability(id, originalId);

        public async Task<Result<Nothing>> CheckApplicability(string description) => await applicabilityRepo.CheckApplicability(description);

        public async Task<Result<Nothing>> DeleteApplicability(int id) => await applicabilityRepo.DeleteApplicability(id);
        public async Task<Result<Nothing>> DeleteOriginalFromApplicability(int id, int originalId) => await applicabilityRepo.DeleteOriginalFromApplicability(id, originalId);

        public async Task<Result<ApplicabilityDto>> GetApplicabilityAsync(int id) => await applicabilityRepo.GetApplicabilityAsync(id);
        public async Task<Result<List<ApplicabilityDto>>> GetApplicabilityListAsync() => await applicabilityRepo.GetApplicabilityListAsync();
        public async Task<Result<List<ApplicabilityDto>>> GetApplicabilityListByOriginal(int originalId) => await applicabilityRepo.GetApplicabilityListByOriginal(originalId);
        public async Task<Result<List<ApplicabilityDto>>> GetFreeApplicabilityList(int originalId) => await applicabilityRepo.GetFreeApplicabilityList(originalId);

        public async Task<Result<ApplicabilityDto>> UpsertApplicability(ApplicabilityDto applicability) => await applicabilityRepo.UpsertApplicability(applicability);
    }
}
