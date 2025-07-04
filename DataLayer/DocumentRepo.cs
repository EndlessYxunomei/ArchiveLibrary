using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataLayer;

public class DocumentRepo(ArchiveDbContext context) : IDocumentRepo
{
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<List<DocumentListDto>>> GetDocumentListAsync(DocumentType type)
    {
        try
        {
            var res = await _context.Documents.AsNoTracking().Where(x => x.DocumentType == type).Select(s => (DocumentListDto)s).ToListAsync();
            return Result<List<DocumentListDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<DocumentListDto>>.Fail(ex);
        }
    }
}
