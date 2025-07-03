using ArchiveDB;
using ArchiveModels;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DataLayer;

public class DocumentRepo(ArchiveDbContext context) : IDocumentRepo
{
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<List<Document>>> GetDocumentListAsync(DocumentType type)
    {
        try
        {
            var res = await _context.Documents.AsNoTracking().Where(x => x.DocumentType == type).ToListAsync();
            return Result<List<Document>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<Document>>.Fail(ex);
        }
    }
}
