using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;

namespace ServiceLayer.Interfaces;

public interface IDocumentService
{
    public Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type);
    public Task<Result<List<DocumentListDto>>> GetDocumentListAsync();
    public Task<Result<DocumentListDto>> UpsertDocument(DocumentDetailDto document);
    public Task<Result<Nothing>> DeleteDocument(int id);
    public Task<Result<DocumentDetailDto>> GetDocumentDetailAsync(int id);
    public Task<Result<Nothing>> CheckDocument(string name, DateOnly date);
}
