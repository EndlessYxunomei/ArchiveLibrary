using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IPersonRepo
{
    public Task<Result<List<Person>>> GetPersonListAsync();
}
