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
    
    public async Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type)
    {
        var document_list = await documentRepo.GetDocumentListAsync(type);
        return document_list.IsSuccess
            ? Result<List<DocumentListDto>>.Success(document_list.Data)
            : Result<List<DocumentListDto>>.Fail(document_list.ErrorCode, document_list.ErrorData, document_list.Exception);
    }
}
