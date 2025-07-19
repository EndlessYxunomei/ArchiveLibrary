using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class ApplicabilityRepo(ArchiveDbContext context) : IApplicabilityRepo
{
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<Nothing>> AddOriginalToApplicability(int id, int originalId)
    {
        try
        {
            var exist_applicability = await _context.Applicabilities
                .FirstOrDefaultAsync(x => x.Id == id);
            var newOriginal = await _context.Originals
                .FirstOrDefaultAsync(x => x.Id == originalId);

            if (exist_applicability == null || newOriginal == null || exist_applicability.Originals.Any(x => x.Id == newOriginal.Id) == false)
            {
                return Result<Nothing>.Fail("Can not add Applicability", $"Applicability Repo. Applicability id={id}, original id={originalId} not found"
                    + " or original already has this applicability.");
            }

            exist_applicability.Originals.Add(newOriginal);
            await _context.SaveChangesAsync();

            return Result<Nothing>.Success();
        }
        catch(Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> CheckApplicability(string description)
    {
        try
        {
            bool result = await _context.Applicabilities.AnyAsync(x => x.Description == description);
            return result
                ? Result<Nothing>.Fail("Applicability already exists", $"Applicability Repo. Requested Applicability {description} already exists")
                : Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> DeleteApplicability(int id)
    {
        try
        {
            var applicability = await _context.Applicabilities.FirstOrDefaultAsync(x => x.Id == id);
            if (applicability == null)
            {
                return Result<Nothing>.Fail("Applicability not found", $"Applicability Repo. Applicability id={id} is not found during delete procedure");
            }
            applicability.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
    public async Task<Result<Nothing>> DeleteOriginalFromApplicability(int id, int originalId)
    {
        try
        {
            var exist_applicability = await _context.Applicabilities
                .FirstOrDefaultAsync(x => x.Id == id);

            if (exist_applicability == null)
            {
                return Result<Nothing>.Fail("Can not delete Applicability", $"Applicability Repo. Applicability id={id} not found");
            }

            int res = exist_applicability.Originals.RemoveAll(x => x.Id == originalId);
            await _context.SaveChangesAsync();

            return res > 0
                ? Result<Nothing>.Success()
                : Result<Nothing>.Fail("Nothing to delete", $"Applicability Repo. Original id={originalId} not have this applicability");

        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }

    public async Task<Result<ApplicabilityDto>> GetApplicabilityAsync(int id)
    {
        try
        {
            var appl = await _context.Applicabilities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return appl != null
                ? Result<ApplicabilityDto>.Success((ApplicabilityDto)appl)
                : Result<ApplicabilityDto>.Fail("Applicability not found", $"Applicability repo. Applicability id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<ApplicabilityDto>.Fail(ex);
        }
    }

    public async Task<Result<List<ApplicabilityDto>>> GetApplicabilityListAsync()
    {
        try
        {
            var res = await _context.Applicabilities
                .AsNoTracking()
                .Select(x => (ApplicabilityDto)x)
                .ToListAsync();
            return Result<List<ApplicabilityDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<ApplicabilityDto>>.Fail(ex);
        }
    }
    public async Task<Result<List<ApplicabilityDto>>> GetApplicabilityListByOriginal(int originalId)
    {
        try
        {
            var res = await _context.Applicabilities
                .AsNoTracking()
                .Where(x => x.Originals.Contains(_context.Originals.First(x => x.Id == originalId)))
                .Select(x => (ApplicabilityDto)x)
                .ToListAsync();
            return Result<List<ApplicabilityDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<ApplicabilityDto>>.Fail(ex);
        }
    }
    public async Task<Result<List<ApplicabilityDto>>> GetFreeApplicabilityList(int originalId)
    {
        try
        {
            var res = await _context.Applicabilities
                .AsNoTracking()
                .Except(_context.Applicabilities.Where(x => x.Originals.Contains(_context.Originals.First(y => y.Id == originalId))))
                .Select(x => (ApplicabilityDto)x)
                .ToListAsync();
            return Result<List<ApplicabilityDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<ApplicabilityDto>>.Fail(ex);
        }
    }

    public async Task<Result<ApplicabilityDto>> UpsertApplicability(ApplicabilityDto applicability)
    {
        return applicability.Id > 0
            ? await UpdateApplicability(applicability)
            : await CreateApplicability(applicability);
    }
    private async Task<Result<ApplicabilityDto>> CreateApplicability(ApplicabilityDto applicability)
    {
        try
        {
            Applicability new_applicability = (Applicability)applicability;
            new_applicability.CreatedDate = DateTime.Now;
            await _context.Applicabilities.AddAsync(new_applicability);
            await _context.SaveChangesAsync();

            return new_applicability.Id > 0
                ? Result<ApplicabilityDto>.Success((ApplicabilityDto)new_applicability)
                : Result<ApplicabilityDto>.Fail("Can't create Applicability", $"Applicability Repo. Cannot create applicability {new_applicability.Description}");
        }
        catch (Exception ex)
        {
            return Result<ApplicabilityDto>.Fail(ex);
        }
    }
    private async Task<Result<ApplicabilityDto>> UpdateApplicability(ApplicabilityDto applicability)
    {
        try
        {
            Applicability updated_applicability = (Applicability)applicability;
            var dbApplicability = await _context.Applicabilities
                .FirstOrDefaultAsync(x => x.Id == applicability.Id);
            if (dbApplicability == null)
            {
                return Result<ApplicabilityDto>.Fail("Applicability not found", $"Applicability Repo. Applicability id={updated_applicability.Id} is not found during update procedure");
            }

            dbApplicability.Description = updated_applicability.Description;

            dbApplicability.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Result<ApplicabilityDto>.Success((ApplicabilityDto)dbApplicability);
        }
        catch (Exception ex)
        {
            return Result<ApplicabilityDto>.Fail(ex);
        }
    }
}
