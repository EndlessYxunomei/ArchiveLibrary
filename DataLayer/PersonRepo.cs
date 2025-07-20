using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class PersonRepo(ArchiveDbContext context) : IPersonRepo
{   
    private readonly ArchiveDbContext _context = context;

    public async Task<Result<Nothing>> CheckPersonFullName(string lastName, string? firstName)
    {
        try
        {
            bool result = await _context.People.AnyAsync(x => x.LastName == lastName && x.FirstName == firstName);
            return result
                ? Result<Nothing>.Fail("Person with same name exists", $"Person Repo. Requested person {lastName} {firstName} already exists")
                : Result<Nothing>.Success();
        }
        catch(Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
        throw new NotImplementedException();
    }

    public async Task<Result<PersonListDto>> GetPersonAsync(int id)
    {
        try
        {
            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return person != null
                ? Result<PersonListDto>.Success((PersonListDto)person)
                : Result<PersonListDto>.Fail("Person not found", $"Person repo. Person id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<PersonListDto>.Fail(ex);
        }
        throw new NotImplementedException();
    }
    public async Task<Result<PersonDetailDto>> GetPersonDetailAsync(int id)
    {
        try
        {
            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return person != null
                ? Result<PersonDetailDto>.Success((PersonDetailDto)person)
                : Result<PersonDetailDto>.Fail("Person not found", $"Person repo. Person id={id} is not found in database");
        }
        catch (Exception ex)
        {
            return Result<PersonDetailDto>.Fail(ex);
        }
        throw new NotImplementedException();
    }

    public async Task<Result<List<PersonListDto>>> GetPersonListAsync()
    {
        try
        {
            var res = await _context.People.AsNoTracking().Select(s => (PersonListDto)s).ToListAsync();
            return Result<List<PersonListDto>>.Success(res);
        }
        catch (Exception ex)
        {
            return Result<List<PersonListDto>>.Fail(ex);
        }
    }

    public async Task<Result<int>> UpsertPerson(PersonDetailDto person)
    {
        return person.Id > 0
        ? await UpdatePerson(person)
        : await CreatePerson(person);
    }
    private async Task<Result<int>> CreatePerson(PersonDetailDto person)
    {
        try
        {
            Person new_person = (Person)person;
            new_person.CreatedDate = DateTime.Now;
            await _context.People.AddAsync(new_person);
            await _context.SaveChangesAsync();

            return new_person.Id > 0
                ? Result<int>.Success(new_person.Id)
                : Result<int>.Fail("Can't create Person", $"Person Repo. Cannot create Person {new_person.LastName} {new_person.FirstName}");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }
    private async Task<Result<int>> UpdatePerson(PersonDetailDto person)
    {
        try
        {
            Person updated_person = (Person)person;
            var dbPerson = await _context.People.FirstOrDefaultAsync(x => x.Id == updated_person.Id);
            if (dbPerson == null)
            {
                return Result<int>.Fail("Person not found", $"Person Repo. Person id={person.Id} is not found during update procedure");
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Department = person.Department;

            dbPerson.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Result<int>.Success(dbPerson.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail(ex);
        }
    }

    public async Task<Result<Nothing>> DeletePerson(int id)
    {
        try
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return Result<Nothing>.Fail("Person not found", $"Person Repo. Person id={id} is not found during delete procedure");
            }
            person.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result<Nothing>.Success();
        }
        catch (Exception ex)
        {
            return Result<Nothing>.Fail(ex);
        }
    }
}
