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

    public async Task<Result<List<PersonListDto>>> GetPersonListAsync() => await personRepo.GetPersonListAsync();
}
