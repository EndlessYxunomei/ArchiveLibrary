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

    public async Task<Result<CompanyDto>> GetCompanyAsync(int companyId)
    {
        var res = await companyRepo.GetCompanyAsync(companyId);
        return res.IsSuccess
            ? Result<CompanyDto>.Success(res.Data)
            : Result<CompanyDto>.Fail(res.ErrorCode, res.ErrorData, res.Exception); ;
    }

    public async Task<Result<List<CompanyDto>>> GetCompanyListAsync()
    {
        var company_list = await companyRepo.GetCompanyListAsync();
        return company_list.IsSuccess
            ? Result<List<CompanyDto>>.Success(company_list.Data)
            : Result<List<CompanyDto>>.Fail(company_list.ErrorCode, company_list.ErrorData, company_list.Exception);
    }

    public async Task<Result<CompanyDto>> UpsertCompany(CompanyDto companyDto)
    {
        var newCompany = await companyRepo.UpsertCompany(companyDto);
        return newCompany.IsSuccess
            ? Result<CompanyDto>.Success(newCompany.Data)
            : Result<CompanyDto>.Fail(newCompany.ErrorCode, newCompany.ErrorData, newCompany.Exception);
    }

    public Task<Result<Nothing>> DeleteCompany(int companyId) => companyRepo.DeleteCompany(companyId);
}

