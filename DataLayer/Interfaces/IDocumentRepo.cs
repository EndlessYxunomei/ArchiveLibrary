using ArchiveModels.DTO;
using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IDocumentRepo
{
    public Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type);
}
