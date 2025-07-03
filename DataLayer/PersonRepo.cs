using ArchiveDB;
using ArchiveModels;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class PersonRepo(ArchiveDbContext context) : IPersonRepo
{   
    private readonly ArchiveDbContext _context = context;
    
    public async Task<Result<List<Person>>> GetPersonListAsync()
    {
        try
        {
            var res = await _context.People.AsNoTracking().ToListAsync();
            return Result<List<Person>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<Person>>.Fail("Ошибка получения списка пользователей",null,ex);
        }
    }
}
