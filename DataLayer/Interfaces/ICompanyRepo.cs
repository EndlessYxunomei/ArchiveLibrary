using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface ICompanyRepo
{
    public Task<Result<List<CompanyDto>>> GetCompanyListAsync();
    public Task<Result<CompanyDto>> GetCompanyAsync(int id);
    public Task<Result<Nothing>> CheckCompany(string name);
    public Task<Result<CompanyDto>> UpsertCompany(CompanyDto company);
    public Task<Result<Nothing>> DeleteCompany(int id);
}
