using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface ICompanyService
{
    public Task<Result<List<CompanyDto>>> GetCompanyListAsync();
    public Task<Result<CompanyDto>> GetCompanyAsync(int companyId);
    public Task<Result<CompanyDto>> UpsertCompany(CompanyDto companyDto);
    public Task<Result<Nothing>> DeleteCompany(int companyId);
    public Task<Result<Nothing>> CheckCompany(string name);
}
