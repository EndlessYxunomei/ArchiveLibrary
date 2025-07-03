using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface ICompanyService
{
    public Task<Result<List<CompanyDto>>> GetCompanyListAsync();
}
