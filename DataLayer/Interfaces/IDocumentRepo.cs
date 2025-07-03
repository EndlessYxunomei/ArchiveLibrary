using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IDocumentRepo
{
    public Task<Result<List<Document>>> GetDocumentListAsync(DocumentType type);
}
