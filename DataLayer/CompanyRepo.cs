using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class CompanyRepo(ArchiveDbContext context) : ICompanyRepo
{
    private readonly ArchiveDbContext _context = context;

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
}
