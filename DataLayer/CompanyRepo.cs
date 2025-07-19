using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class CompanyRepo(ArchiveDbContext context) : ICompanyRepo
{
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<CompanyDto>> GetCompanyAsync(int id)
    {
        try
        {
            var company = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return company != null
                ? Result<CompanyDto>.Success((CompanyDto)company)
                : Result<CompanyDto>.Fail("Company not found", $"Company repo. Company id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Fail(ex);
        }
    }

    public async Task<Result<List<CompanyDto>>> GetCompanyListAsync()
    {
        try
        {
            var res = await _context.Companies.AsNoTracking().Select(s => (CompanyDto)s).ToListAsync();
            return Result<List<CompanyDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<CompanyDto>>.Fail(ex);
        }

    }

    public async Task<Result<CompanyDto>> UpsertCompany(CompanyDto company)
    {
        return company.Id > 0
            ? await UpdateCompany(company)
            : await CreateCompany(company);
    }
    private async Task<Result<CompanyDto>> CreateCompany(CompanyDto company)
    {
        try
        {
            Company new_company = (Company)company;
            new_company.CreatedDate = DateTime.Now;
            await _context.Companies.AddAsync(new_company);
            await _context.SaveChangesAsync();

            return new_company.Id > 0
                ? Result<CompanyDto>.Success((CompanyDto)new_company)
                : Result<CompanyDto>.Fail("Can't create Company", $"Company Repo. Cannot create company {new_company.Name}");
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Fail(ex);
        }
    }
    private async Task<Result<CompanyDto>> UpdateCompany(CompanyDto company)
    {
        try
        {
            Company updated_company = (Company)company;
            var dbCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Id == updated_company.Id);
            if (dbCompany == null)
            {
                return Result<CompanyDto>.Fail("Company not found", $"Company Repo. Company id={company.Id} is not found during update procedure");
            }

            dbCompany.Name = updated_company.Name;
            dbCompany.Description = updated_company.Description;
            dbCompany.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Result<CompanyDto>.Success((CompanyDto)dbCompany);
        }
        catch (Exception ex)
        {
            return Result<CompanyDto>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> DeleteCompany(int id)
    {
        try
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return Result<Nothing>.Fail("Company not found", $"Company Repo. Company id={id} is not found during delete procedure");
            }
            company.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
    public async Task<Result<Nothing>> CheckCompany(string name)
    {
        try
        {
            bool result = await _context.Companies.AnyAsync(x => x.Name == name);
            return result
                ? Result<Nothing>.Fail("Company with same name already exists", $"Company Repo. Requested company with {name} already exists")
                : Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
}
