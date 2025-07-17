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

    public async Task<Result<DocumentListDto>> GetDocumentAsync(int id)
    {
        try
        {
            var document = await _context.Documents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return document != null
                ? Result<DocumentListDto>.Success((DocumentListDto)document)
                : Result<DocumentListDto>.Fail("Document not found", $"Document repo. Document id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<DocumentListDto>.Fail(ex);
        }
    }
    public async Task<Result<DocumentDetailDto>> GetDocumentDetailAsync(int id)
    {
        try
        {
            var document = await _context.Documents
                .Include(x => x.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return document != null
                ? Result<DocumentDetailDto>.Success((DocumentDetailDto)document)
                : Result<DocumentDetailDto>.Fail("Document not found", $"Document repo. Document id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<DocumentDetailDto>.Fail(ex);
        }
    }

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
    public async Task<Result<List<DocumentListDto>>> GetDocumentListAsync()
    {
        try
        {
            var res = await _context.Documents.AsNoTracking().Select(s => (DocumentListDto)s).ToListAsync();
            return Result<List<DocumentListDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<DocumentListDto>>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> CheckDocument(string name, DateOnly date)
    {
        try
        {
            bool result = await _context.Documents.AnyAsync(x => x.Name == name && x.Date == date);
            return result
                ? Result<Nothing>.Fail("Document with same number and date already exists", $"Document Repo. Requested docunent with {name} dated {date: d} already exists")
                : Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<int>> UpsertDocument(DocumentDetailDto document)
    {
        try
        {
            return document.Id > 0
                ? await UpdateDocument(document)
                : await CreateDocument(document);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }
    private async Task<Result<int>> CreateDocument(DocumentDetailDto document)
    {
        try
        {
            Document new_document = (Document)document;
            new_document.CreatedDate = DateTime.Now;
            await _context.Documents.AddAsync(new_document);
            await _context.SaveChangesAsync();

            return new_document.Id > 0
                ? Result<int>.Success(new_document.Id)
                : Result<int>.Fail("Can't create Document", $"Document Repo. Cannot create document {new_document.Name} dated {new_document.Date : d}");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }
    private async Task<Result<int>> UpdateDocument(DocumentDetailDto document)
    {
        try
        {
            Document updated_document = (Document)document;
            var dbDocument = await _context.Documents.FirstOrDefaultAsync(x => x.Id == updated_document.Id);
            if (dbDocument == null)
            {
                return Result<int>.Fail("Document not found", $"Document Repo. Document id={document.Id} is not found during update procedure");
            }

            dbDocument.Name = updated_document.Name;
            dbDocument.Date = updated_document.Date;
            dbDocument.Description = updated_document.Description;
            dbDocument.CompanyId = updated_document.CompanyId;
            dbDocument.DocumentType = updated_document.DocumentType;

            dbDocument.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Result<int>.Success(dbDocument.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> DeleteDocument(int id)
    {
        try
        {
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
            if (document == null)
            {
                return Result<Nothing>.Fail("Document not found", $"Document Repo. Document id={id} is not found during delete procedure");
            }
            document.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
}
