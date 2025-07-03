using ArchiveDB;
using ArchiveModels;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class CompanyRepo(ArchiveDbContext context) : ICompanyRepo
{
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<List<Company>>> GetCompanyListAsync()
    {
        try
        {
            var res = await _context.Companies.AsNoTracking().ToListAsync();
            return Result<List<Company>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<Company>>.Fail("Ошибка получения списка компаний", null, ex);
        }
    }
}
