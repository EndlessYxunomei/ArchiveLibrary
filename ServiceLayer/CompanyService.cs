using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepo companyRepo;
    public CompanyService(ICompanyRepo companyRepo)
    {
        this.companyRepo = companyRepo;
    }
    public CompanyService(ArchiveDbContext context)
    {
        companyRepo = new CompanyRepo(context);
    }

    public async Task<Result<CompanyDto>> GetCompanyAsync(int companyId) => await companyRepo.GetCompanyAsync(companyId);

    public async Task<Result<List<CompanyDto>>> GetCompanyListAsync() => await companyRepo.GetCompanyListAsync();

    public async Task<Result<CompanyDto>> UpsertCompany(CompanyDto companyDto) => await companyRepo.UpsertCompany(companyDto);

    public Task<Result<Nothing>> DeleteCompany(int companyId) => companyRepo.DeleteCompany(companyId);

    public Task<Result<Nothing>> CheckCompany(string name) => companyRepo.CheckCompany(name);
}

