using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer;
using DataLayer.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepo documentRepo;
    public DocumentService(IDocumentRepo documentRepo)
    {
        this.documentRepo = documentRepo;
    }
    public DocumentService(ArchiveDbContext context)
    {
        documentRepo = new DocumentRepo(context);
    }

    public async Task<Result<Nothing>> CheckDocument(string name, DateOnly date) => await documentRepo.CheckDocument(name, date);

    public async Task<Result<Nothing>> DeleteDocument(int id) => await documentRepo.DeleteDocument(id);

    public async Task<Result<DocumentDetailDto>> GetDocumentDetailAsync(int id) => await documentRepo.GetDocumentDetailAsync(id);

    public async Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type) => await documentRepo.GetDocumentListAsync(type);
    public async Task<Result<List<DocumentListDto>>> GetDocumentListAsync() => await documentRepo.GetDocumentListAsync();

    public async Task<Result<DocumentListDto>> UpsertDocument(DocumentDetailDto document)
    {
        var newDicumentId = await documentRepo.UpsertDocument(document);
        if (newDicumentId.IsSuccess)
        {
            var newDocument = await documentRepo.GetDocumentAsync(newDicumentId.Data);
            if (newDocument.IsSuccess)
            {
                return Result<DocumentListDto>.Success(newDocument.Data);
            }
            return Result<DocumentListDto>.Fail(newDocument.ErrorCode, newDocument.ErrorData, newDocument.Exception);
        }
        return Result<DocumentListDto>.Fail(newDicumentId.ErrorCode, newDicumentId.ErrorData, newDicumentId.Exception);
    }
}
