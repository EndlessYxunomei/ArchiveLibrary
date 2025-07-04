using ArchiveDB;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;


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
        return person_list.IsSuccess
            ? Result<List<PersonListDto>>.Success(person_list.Data)
            : Result<List<PersonListDto>>.Fail(person_list.ErrorCode, person_list.ErrorData, person_list.Exception);
    }
}
