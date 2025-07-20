using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface IPersonService
{
    public Task<Result<List<PersonListDto>>> GetPersonListAsync();
    public Task<Result<PersonDetailDto>> GetPersonDetailAsync(int id);
    public Task<Result<Nothing>> CheckPersonFullName(string lastName, string? firstName);
    public Task<Result<PersonListDto>> UpsertPerson(PersonDetailDto person);
    public Task<Result<Nothing>> DeletePerson(int id);
}
