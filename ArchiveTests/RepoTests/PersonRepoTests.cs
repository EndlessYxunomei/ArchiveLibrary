using ArchiveDB;
using DataLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;


namespace ArchiveTests.RepoTests;

public class PersonRepoTests: IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ArchiveDbContext> _contextOptions;

    #region ConstructorAndDispose
    public PersonRepoTests()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<ArchiveDbContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new ArchiveDbContext(_contextOptions);

//        if (context.Database.EnsureCreated())
//        {
//            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
//            viewCommand.CommandText = @"
//CREATE VIEW AllResources AS
//SELECT Url
//FROM Blogs;";
//            viewCommand.ExecuteNonQuery();
//        }

        context.People.AddRange(
            new() { LastName = "Пестрецов", FirstName = "Глеб", Department = "ПО" },
            new() { LastName = "Шестаков", FirstName = "Александр", Department = "ОМТО" },
            new() { LastName = "Крутик", FirstName = "Владимир", Department = "ПБ" },
            new() { LastName = "Царев", FirstName = "Алексей", Department = "ПО" });
        context.SaveChanges();
    }

    ArchiveDbContext CreateContext() => new(_contextOptions);

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion


    [Fact]
    public async Task PersonListDtoCreatedCorrectly()
    {
        //Arrange
        /**
         * //создаем фальшивый датасет для фальшивой базы
        var data = new List<Person>()
        {
            new() { LastName = "Пестрецов", FirstName = "Глеб", Department = "ПО"},
            new() { LastName = "Шестаков", FirstName = "Александр", Department = "ОМТО"},
            new() { LastName = "Крутик", FirstName = "Владимир", Department = "ПБ"},
            new() { LastName = "Царев", FirstName = "Алексей", Department = "ПО"}
        }.AsQueryable();
        //используем самописный класс для задания всех параметров фальшивого датасета для асинхронного чтения
        //подробности в https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
        //а также https://sinairv.github.io/blog/2015/10/04/mock-entity-framework-dbset-with-nsubstitute/
        var mockSet = NSubstituteUtils.CreateMockDbSet(data);

        //создаем и настраиваем фальшивый дбконтекст
        var mockContext = Substitute.For<ArchiveDbContext>();
        mockContext.People.Returns(mockSet);
        //создаем объект для тестирования
        var personRepo = new PersonRepo(mockContext);
        **/
        using var context = CreateContext();
        var personRepo = new PersonRepo(context);

        //Act
        var test_person_list = await personRepo.GetPersonListAsync();

        //Assert
        Assert.True(test_person_list.IsSuccess);
        Assert.Equal(4, test_person_list.Data.Count);
    }
}
