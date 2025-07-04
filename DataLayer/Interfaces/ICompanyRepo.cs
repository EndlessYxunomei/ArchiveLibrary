using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface ICompanyRepo
{
    public Task<Result<List<CompanyDto>>> GetCompanyListAsync();
}
