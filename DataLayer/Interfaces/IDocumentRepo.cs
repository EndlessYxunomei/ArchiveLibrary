using ArchiveModels.DTO;
using ArchiveModels;
using ArchiveModels.Utilities;

namespace DataLayer.Interfaces;

public interface IDocumentRepo
{
    public Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type);
    public Task<Result<DocumentDetailDto>> GetDocumentDetailAsync(int id);
    public Task<Result<DocumentListDto>> GetDocumentAsync(int id);
    public Task<Result<Nothing>> DeleteDocument(int id);
    public Task<Result<Nothing>> CheckDocument(string name, DateOnly date);
    public Task<Result<int>> UpsertDocument(DocumentDetailDto document);
}
