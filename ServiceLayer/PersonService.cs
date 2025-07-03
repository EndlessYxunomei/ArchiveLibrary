using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer;

public class PersonService : IPersonService
{
    private readonly IPersonRepo personRepo;
    public PersonService(IPersonRepo personRepo)
    {
        this.personRepo = personRepo;
    }
    public PersonService(ArchiveDbContext context)
    {
        personRepo = new PersonRepo(context);
    }
    
    public async Task<Result<List<PersonListDto>>> GetPersonListAsync()
    {
        var person_list = await personRepo.GetPersonListAsync();
        if (person_list.IsSuccess)
        {
            List<PersonListDto> list = [];
            foreach (var person in person_list.Data)
            {
                PersonListDto dto = (PersonListDto)person;
                list.Add(dto);
            }
            return Result<List<PersonListDto>>.Success(list);
        }

        return Result<List<PersonListDto>>.Fail(person_list.ErrorCode, person_list.ErrorData, person_list.Exception);
    }
}
