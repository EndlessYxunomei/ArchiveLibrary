using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IPersonRepo
{
    public Task<Result<List<PersonListDto>>> GetPersonListAsync();
}
