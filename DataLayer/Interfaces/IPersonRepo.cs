using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IPersonRepo
{
    public Task<Result<List<PersonListDto>>> GetPersonListAsync();
    public Task<Result<PersonListDto>> GetPersonAsync(int id);
    public Task<Result<PersonDetailDto>> GetPersonDetaliAsync(int id);
    public Task<Result<Nothing>> CheckPersonFullName(string lastName, string? firstName);
    public Task<Result<int>> UpsertPerson(PersonDetailDto person);
    public Task<Result<Nothing>> DeletePerson(int id);
}
