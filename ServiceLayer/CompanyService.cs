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

    public async Task<Result<List<CompanyDto>>> GetCompanyListAsync()
    {
        var company_list = await companyRepo.GetCompanyListAsync();
        return company_list.IsSuccess
            ? Result<List<CompanyDto>>.Success(company_list.Data)
            : Result<List<CompanyDto>>.Fail(company_list.ErrorCode, company_list.ErrorData, company_list.Exception);
    }
}

