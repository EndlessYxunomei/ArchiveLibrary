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
        if (company_list.IsSuccess)
        {
            List<CompanyDto> list = [];
            foreach (var company in company_list.Data)
            {
                CompanyDto dto = (CompanyDto)company;
                list.Add(dto);
            }
            return Result<List<CompanyDto>>.Success(list);
        }
        return Result<List<CompanyDto>>.Fail(company_list.ErrorCode, company_list.ErrorData, company_list.Exception);
    }
}

