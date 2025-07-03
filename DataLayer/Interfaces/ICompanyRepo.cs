using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface ICompanyRepo
{
    public Task<Result<List<Company>>> GetCompanyListAsync();
}
