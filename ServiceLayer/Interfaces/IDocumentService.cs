using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface IDocumentService
{
    public Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type);
}
