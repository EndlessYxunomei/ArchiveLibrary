using ArchiveDB;
using ArchiveModels;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class OriginalRepo(ArchiveDbContext context) : IOriginalRepo
{
    public readonly ArchiveDbContext _context = context;

    //старые реализации
    //public async Task<Original> GetOriginalAsync(int id)
    //{
    //    var original = await _context.Originals
    //        .Include(x => x.Copies)
    //        .Include(x => x.Applicabilities)
    //        .Include(x => x.Corrections)
    //        .Include(x => x.Company)
    //        .Include(x => x.Document)
    //        .Include(x => x.Person)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(x => x.Id == id);
    //    return original ?? throw new Exception("Original not found");
    //}
    //public async Task<List<Original>> GetOriginalList()
    //{
    //    return await _context.Originals.AsNoTracking().Include(x => x.Document).OrderBy(x => x.InventoryNumber).ToListAsync();
    //}
    //public async Task<List<Original>> GetOriginalsByDocument(int docunentId)
    //{
    //    return await _context.Originals.AsNoTracking().Where(x => x.DocumentId == docunentId).ToListAsync();
    //}

    //public async Task<int> UpsertOriginal(Original original)
    //{
    //    if (original.Id > 0)
    //    {
    //        return await UpdateOriginal(original);
    //    }
    //    return await CreateOriginal(original);
    //}
    //private async Task<int> CreateOriginal(Original original)
    //{
    //    original.CreatedDate = DateTime.Now;
    //    await _context.Originals.AddAsync(original);
    //    await _context.SaveChangesAsync();
    //    if (original.Id <= 0) { throw new Exception("Could not Create the original as expected"); }
    //    return original.Id;
    //}
    //private async Task<int> UpdateOriginal(Original original)
    //{
    //    var dbOriginal = await _context.Originals
    //        //.Include(x => x.Applicabilities)
    //        //.Include(x => x.Corrections)
    //        //.Include(x => x.Copies)
    //        .FirstOrDefaultAsync(x => x.Id == original.Id) ?? throw new Exception("Original not found");

    //    dbOriginal.Caption = original.Caption;
    //    dbOriginal.Name = original.Name;
    //    dbOriginal.InventoryNumber = original.InventoryNumber;
    //    dbOriginal.IsDeleted = original.IsDeleted;
    //    dbOriginal.Notes = original.Notes;
    //    dbOriginal.PageCount = original.PageCount;
    //    dbOriginal.PageFormat = original.PageFormat;
    //    dbOriginal.DocumentId = original.DocumentId;
    //    dbOriginal.CompanyId = original.CompanyId;
    //    dbOriginal.PersonId = original.PersonId;
    //    dbOriginal.LastModifiedDate = DateTime.Now;
    //    /*if (original.Applicabilities != null)
    //    {
    //        dbOriginal.Applicabilities = original.Applicabilities;
    //    }
    //    if (original.Corrections != null)
    //    {
    //        dbOriginal.Corrections = original.Corrections;
    //    }
    //    if (original.Copies != null)
    //    {
    //        dbOriginal.Copies = original.Copies;
    //    }*/

    //    await _context.SaveChangesAsync();
    //    return original.Id;
    //}
    ////не используется
    //public async Task UpsertOriginals(List<Original> originals)
    //{
    //    using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
    //    try
    //    {
    //        foreach (var original in originals)
    //        {
    //            var success = await UpsertOriginal(original) > 0;
    //            if (!success) { throw new Exception($"Error saving the original {original.Name}"); }
    //        }
    //        await transaction.CommitAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.ToString());
    //        await transaction.RollbackAsync();
    //        throw;
    //    }
    //    //не работает в SQLite
    //    /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
    //        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
    //        TransactionScopeAsyncFlowOption.Enabled))
    //    {
    //        try
    //        {
    //            foreach (var original in originals)
    //            {
    //                var success = await UpsertOriginal(original) > 0;
    //                if (!success) { throw new Exception($"Error saving the original {original.Name}"); }
    //            }
    //            scope.Complete();
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.ToString());
    //            throw;
    //        }
    //    }*/
    //}
    //public async Task DeleteOriginal(int id)
    //{
    //    var original = await _context.Originals.FirstOrDefaultAsync(x => x.Id == id);
    //    if (original == null) { return; }
    //    original.IsDeleted = true;
    //    await _context.SaveChangesAsync();
    //}
    ////не используется
    //public async Task DeleteOriginals(List<int> originalIds)
    //{
    //    using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
    //    try
    //    {
    //        foreach (var originalId in originalIds)
    //        {
    //            await DeleteOriginal(originalId);
    //        }
    //        await transaction.CommitAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.ToString());
    //        await transaction.RollbackAsync();
    //        throw;
    //    }
    //    //не работает в SQLite
    //    /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
    //        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
    //        TransactionScopeAsyncFlowOption.Enabled))
    //    {
    //        try
    //        {
    //            foreach (var originalId in originalIds)
    //            {
    //                await DeleteOriginal(originalId);
    //            }
    //            scope.Complete();
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.ToString());
    //            throw;
    //        }
    //    }*/
    //}

    //public async Task<int> GetLastInventoryNumberAsync()
    //{
    //    return await _context.Originals.MaxAsync(y => y.InventoryNumber);
    //}
    //public async Task<bool> CheckInventoryNumberAsync(int inventoryNumber)
    //{
    //    bool result = await _context.Originals.AnyAsync(x => x.InventoryNumber == inventoryNumber);
    //    return !result;
    //}

    //public async Task<List<Original>> GetOriginalsByCompany(int companyId)
    //{
    //    return await _context.Originals.AsNoTracking().Where(x => x.CompanyId == companyId).ToListAsync();
    //}

    //public async Task UpdateOriginalApplicabilities(int id, List<int> applicabilityIds)
    //{
    //    var dbOriginal = await _context.Originals.Include(x => x.Applicabilities).FirstOrDefaultAsync(x => x.Id == id);
    //    List<Applicability> dbApps = [];
    //    foreach (var applicability in applicabilityIds)
    //    {
    //        dbApps.Add(await _context.Applicabilities.FirstAsync(y => y.Id == applicability));
    //    }
    //    if (dbOriginal != null)
    //    {
    //        dbOriginal.Applicabilities = dbApps;
    //        await _context.SaveChangesAsync();
    //    }
    //}

    //public async Task<List<Original>> GetOriginalsByApplicability(int applicabilityId)
    //{
    //    return await _context.Originals.AsNoTracking().Where(x => x.Applicabilities.Contains(_context.Applicabilities.First(y => y.Id == applicabilityId))).ToListAsync();
    //}

    //новые реализации интерфейса
    public async Task<Result<Original>> GetOriginalAsync(int id)
    {
        try
        {
            var original = await _context.Originals
                .Include(x => x.Copies)
                .Include(x => x.Applicabilities)
                .Include(x => x.Corrections)
                .Include(x => x.Company)
                .Include(x => x.Document)
                .Include(x => x.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return original != null ? Result<Original>.Success(original) : Result<Original>.Fail("Original not found", $"Original repo. Original id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<Original>.Fail(ex);
        }
    }
    public async Task<Result<List<Original>>> GetOriginalList()
    {
        try
        {
            List<Original> original_list = await _context.Originals.AsNoTracking().Include(x => x.Document).OrderBy(x => x.InventoryNumber).ToListAsync();
            return Result<List<Original>>.Success(original_list);
        }
        catch (Exception ex)
        {
            return Result<List<Original>>.Fail(ex);
        }
    }

    public async Task<Result<List<Original>>> GetOriginalsByDocument(int docunentId)
    {
        try
        {
            List<Original> original_list = await _context.Originals.AsNoTracking().Where(x => x.DocumentId == docunentId).ToListAsync();
            return Result<List<Original>>.Success(original_list);
        }
        catch (Exception ex)
        {
            return Result<List<Original>>.Fail(ex);
        }
    }

    public async Task<Result<List<Original>>> GetOriginalsByCompany(int companyId)
    {
        try
        {
            List<Original> original_list = await _context.Originals.AsNoTracking().Where(x => x.CompanyId == companyId).ToListAsync();
            return Result<List<Original>>.Success(original_list);
        }
        catch (Exception ex)
        {
            return Result<List<Original>>.Fail(ex);
        }
    }

    public async Task<Result<List<Original>>> GetOriginalsByApplicability(int applicabilityId)
    {
        try
        {
            List<Original> original_list = await _context.Originals.AsNoTracking().Where(x => x.Applicabilities
                .Contains(_context.Applicabilities.First(y => y.Id == applicabilityId))).ToListAsync();
            return Result<List<Original>>.Success(original_list);
        }
        catch (Exception ex)
        {
            return Result<List<Original>>.Fail(ex);
        }
    }

    public async Task<Result<int>> GetLastInventoryNumberAsync()
    {
        try
        {
            int max_original_number = await _context.Originals.MaxAsync(y => y.InventoryNumber);
            return Result<int>.Success(max_original_number);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> CheckInventoryNumberAsync(int inventoryNumber)
    {
        try
        {
            bool result = await _context.Originals.AnyAsync(x => x.InventoryNumber == inventoryNumber);
            return result ? Result<Nothing>.Fail("Inventory number is allready occupied", $"Original Repo. Requested inventory number {inventoryNumber} is allready occupied") : Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<int>> UpsertOriginal(Original original)
    {
        try
        {
            return original.Id > 0 ? await UpdateOriginal(original) : await CreateOriginal(original);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }
    private async Task<Result<int>> CreateOriginal(Original original)
    {
        try
        {
            //потом ещё раз попробовать чтобы дата изменений создавалась в контексте автоматически
            original.CreatedDate = DateTime.Now;
            await _context.Originals.AddAsync(original);
            await _context.SaveChangesAsync();

            return original.Id > 0 ? Result<int>.Success(original.Id) : Result<int>.Fail("Can't create Original", $"Original Repo. Cannot create original {original.InventoryNumber} {original.Name}");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }
    private async Task<Result<int>> UpdateOriginal(Original original)
    {
        try
        {
            var dbOriginal = await _context.Originals
            //.Include(x => x.Applicabilities)
            //.Include(x => x.Corrections)
            //.Include(x => x.Copies)
            .FirstOrDefaultAsync(x => x.Id == original.Id);

            if (dbOriginal == null)
            {
                return Result<int>.Fail("Original not found", $"Original Repo. Original id={original.Id} is not found during update procedure");
            }

            dbOriginal.Caption = original.Caption;
            dbOriginal.Name = original.Name;
            dbOriginal.InventoryNumber = original.InventoryNumber;
            dbOriginal.IsDeleted = original.IsDeleted;
            dbOriginal.Notes = original.Notes;
            dbOriginal.PageCount = original.PageCount;
            dbOriginal.PageFormat = original.PageFormat;
            dbOriginal.DocumentId = original.DocumentId;
            dbOriginal.CompanyId = original.CompanyId;
            dbOriginal.PersonId = original.PersonId;

            dbOriginal.LastModifiedDate = DateTime.Now;

            /*if (original.Applicabilities != null)
            {
                dbOriginal.Applicabilities = original.Applicabilities;
            }
            if (original.Corrections != null)
            {
                dbOriginal.Corrections = original.Corrections;
            }
            if (original.Copies != null)
            {
                dbOriginal.Copies = original.Copies;
            }*/

            await _context.SaveChangesAsync();

            return Result<int>.Success(original.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> DeleteOriginal(int id)
    {
        try
        {
            var original = await _context.Originals.FirstOrDefaultAsync(x => x.Id == id);
            if (original == null)
            {
                return Result<Nothing>.Fail("Original not found", $"Original Repo. Original id={id} is not found during delete procedure");
            }
            original.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> UpdateOriginalApplicabilities(int id, List<int> applicabilityIds)
    {
        try
        {
            //сначала находим оригинал
            var dbOriginal = await _context.Originals.Include(x => x.Applicabilities).FirstOrDefaultAsync(x => x.Id == id);
            //теперь получаем новые применимости по их id
            List<Applicability> dbApps = [];
            foreach (var applicability in applicabilityIds)
            {
                dbApps.Add(await _context.Applicabilities.FirstAsync(y => y.Id == applicability));
            }
            //накоенц заменяем оригиналу его применимости новыми
            if (dbOriginal != null)
            {
                dbOriginal.Applicabilities = dbApps;
                await _context.SaveChangesAsync();
                return Result<Nothing>.Success();
            }
            return Result<Nothing>.Fail("Can't update applicabilities", $"Original Repo. Original id={id} not found while udating applicabilities");
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
}
