using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface IPersonService
{
    public Task<Result<List<PersonListDto>>> GetPersonListAsync();
}
