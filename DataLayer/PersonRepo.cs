using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class PersonRepo(ArchiveDbContext context) : IPersonRepo
{   
    private readonly ArchiveDbContext _context = context;
    
    public async Task<Result<List<PersonListDto>>> GetPersonListAsync()
    {
        try
        {
            var res = await _context.People.AsNoTracking().Select(s => (PersonListDto)s).ToListAsync();
            return Result<List<PersonListDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<PersonListDto>>.Fail(ex);
        }
    }

    //var person_list = await personRepo.GetPersonListAsync();
    //    if (person_list.IsSuccess)
    //    {
    //        List<PersonListDto> list = [];
    //        foreach (var person in person_list.Data)
    //        {
    //            PersonListDto dto = (PersonListDto)person;
    //list.Add(dto);
    //        }
    //        return Result<List<PersonListDto>>.Success(list);
    //    }

    //    return Result<List<PersonListDto>>.Fail(person_list.ErrorCode, person_list.ErrorData, person_list.Exception);
    //}
}
