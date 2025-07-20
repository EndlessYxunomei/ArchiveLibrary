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

    public async Task<Result<Nothing>> CheckPersonFullName(string lastName, string? firstName) => await personRepo.CheckPersonFullName(lastName, firstName);

    public async Task<Result<Nothing>> DeletePerson(int id) => await personRepo.DeletePerson(id);

    public async Task<Result<PersonDetailDto>> GetPersonDetaliAsync(int id) => await personRepo.GetPersonDetaliAsync(id);

    public async Task<Result<List<PersonListDto>>> GetPersonListAsync() => await personRepo.GetPersonListAsync();

    public async Task<Result<PersonListDto>> UpsertPerson(PersonDetailDto person)
    {
        var newPersontId = await personRepo.UpsertPerson(person);
        if (newPersontId.IsSuccess)
        {
            var newDocument = await personRepo.GetPersonAsync(newPersontId.Data);
            if (newDocument.IsSuccess)
            {
                return Result<PersonListDto>.Success(newDocument.Data);
            }
            return Result<PersonListDto>.Fail(newDocument.ErrorCode, newDocument.ErrorData, newDocument.Exception);
        }
        return Result<PersonListDto>.Fail(newPersontId.ErrorCode, newPersontId.ErrorData, newPersontId.Exception);
    }
}
