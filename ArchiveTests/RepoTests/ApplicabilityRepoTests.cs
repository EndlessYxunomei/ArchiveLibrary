using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using DataLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ArchiveTests.RepoTests;

public class ApplicabilityRepoTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ArchiveDbContext> _contextOptions;

    #region ConstructorAndDispose
    public ApplicabilityRepoTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<ArchiveDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new ArchiveDbContext(_contextOptions);

        List<Company> TestCompanies =
        [
            new() {Name = "СКБД", Description = "Русский дизель"},
            new() {Name = "завод \"Русский дизель\""},
            new() {Name = "ПО \"Звезда\""},
            new() {Name = "ОАО \"КЗ\""},
            new() {Name = "п/я А-7703", Description = "Звезда"},
            new() {Name = "ВМФ"},
            new() {Name = "Гипрорыбфлот Ленинград"},
            new() {Name = "51 ЦКТИС"},
            new() {Name = "Гипрорыбфлот-сервис"},
            new() {Name = "ГУП ЦКБ \"Черноморец\""},
            new() {Name = "ПАО \"Звезда\""},
            new() {Name = "ФГБУ \"РСТ\""},
            new() {Name = "АО \"ПК \"Звезда\""},
            new() {Name = "ООО \"Компания энергоремонт\""}
        ];
        context.Companies.AddRange(TestCompanies);
        List<Document> TestDocuments =
        [
            new() {Name = "12-53-16", Date = new DateOnly(2000, 12, 7), Company = TestCompanies[0], DocumentType = DocumentType.AddOriginal},
            new() {Name = "72-71б", Date = new DateOnly(2000, 12, 28), Company = TestCompanies[1], DocumentType = DocumentType.AddOriginal},
            new() {Name = "132", Date = new DateOnly(2000, 9, 15), Description = "приложение к письму", Company = TestCompanies[2], DocumentType = DocumentType.AddOriginal},
            new() {Name = "б/н", Date = new DateOnly(2000, 9, 15), Company = TestCompanies[12], DocumentType = DocumentType.CreateCopy},
            new() {Name = "б/н", Date = new DateOnly(2000, 11, 16), Company = TestCompanies[12], DocumentType = DocumentType.CreateCopy},
            new() {Name = "1237", Date = new DateOnly(2000, 9, 15), Company = TestCompanies[13], DocumentType = DocumentType.CreateCopy},
            new() {Name = "б/н", Date = new DateOnly(2001, 9, 15), Company = TestCompanies[12], DocumentType = DocumentType.DeliverCopy},
            new() {Name = "45", Date = new DateOnly(2002, 9, 15), Company = TestCompanies[13], DocumentType = DocumentType.DeliverCopy},
            new() {Name = "14", Date = new DateOnly(2003, 9, 15), Company = TestCompanies[13], DocumentType = DocumentType.DeliverCopy},
            new() {Name = "б/н", Date = new DateOnly(2004, 9, 15), Description = "плохое состояние", Company = TestCompanies[12], DocumentType = DocumentType.DeleteCopy},
            new() {Name = "182", Date = new DateOnly(2023, 9, 15), Description = "Добавление листа 12", Company = TestCompanies[12], DocumentType = DocumentType.AddCorrection},
            new() {Name = "13", Date = new DateOnly(2023, 9, 15), Description = "Исправление опечаток", Company = TestCompanies[13], DocumentType = DocumentType.AddCorrection}
        ];
        context.Documents.AddRange(TestDocuments);
        List<Person> TestPeople =
        [
            new() { LastName = "Пестрецов", FirstName = "Глеб", Department = "ПО"},
            new() { LastName = "Шестаков", FirstName = "Александр", Department = "ОМТО"},
            new() { LastName = "Крутик", FirstName = "Владимир", Department = "ПБ"},
            new() { LastName = "Царев", FirstName = "Алексей", Department = "ПО"}
        ];
        context.People.AddRange(TestPeople);
        List<Applicability> TestApplicabilities =
        [
            new() { Description = "М500"},
            new() { Description = "Русский дизель"},
            new() { Description = "Д42"},
            new() { Description = "Д49"},
            new() { Description = "Д50"},
        ];
        context.Applicabilities.AddRange(TestApplicabilities);
        List<Original> TestOriginals =
        [
            new() { Applicabilities = [TestApplicabilities[2]], Caption = "Элемент протектора", InventoryNumber = 1236, Name = "24.06.14.071", Company = TestCompanies[3], Document = TestDocuments[0], PageCount = 1, PageFormat = "А3", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[2]], Caption = "Дизель 4-2ДЛ42М. Инструкция по техническому обслуживанию", InventoryNumber = 1208, Name = "4-2ДЛ42М.24ИО", Company = TestCompanies[0], Document = TestDocuments[3], PageCount = 401, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[0]], Caption = "Прокладка", InventoryNumber = 1207, Name = "500-15.151", Company = TestCompanies[2], Document = TestDocuments[0], PageCount = 1, PageFormat = "А3", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[0]], Caption = "Поршень", InventoryNumber = 1205, Name = "503-04.101", Company = TestCompanies[2], Document = TestDocuments[0], PageCount = 1, PageFormat = "А4х5", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[2]], Caption = "Крышка цилиндра Сборочный чертеж", InventoryNumber = 1041, Name = "2Д42.78 СБ", Company = TestCompanies[3], Document = TestDocuments[0], PageCount = 2, PageFormat = "А1", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Caption = "Элемент протектора", InventoryNumber = 976, Name = "47Б-128-202", Company = TestCompanies[0], Document = TestDocuments[0], PageCount = 1, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Caption = "Пробка протектора", InventoryNumber = 975, Name = "47Б-128-201", Company = TestCompanies[0], Document = TestDocuments[1], PageCount = 1, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Caption = "Вкладыш шатунный Сборочный чертеж", InventoryNumber = 973, Name = "47Д-038-129 СБ", Company = TestCompanies[1], Document = TestDocuments[0], PageCount = 1, PageFormat = "А2х3", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Caption = "Вкладыш шатунный", InventoryNumber = 972, Name = "47Д-038-129", Company = TestCompanies[0], Document = TestDocuments[1], PageCount = 1, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[2]], Caption = "Вкладыш верхний", InventoryNumber = 886, Name = "7-2Д42.35.11 СПЧ", Company = TestCompanies[0], Document = TestDocuments[1], PageCount = 1, PageFormat = "АF4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[2]], Caption = "Вкладыш нижний", InventoryNumber = 887, Name = "7-2Д42.35.12 СПЧ", Company = TestCompanies[0], Document = TestDocuments[1], PageCount = 1, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Notes = "Аннулирован, замещен инв. № 957 15.08.2017", Caption = "Втулка рабочего цилиндра", InventoryNumber = 201, Name = "68-014-014", Company = TestCompanies[0], Document = TestDocuments[2], PageCount = 1, PageFormat = "А4", Person = TestPeople[0]},
            new() { Applicabilities = [TestApplicabilities[1]], Notes = "Аннулирован, замещен инв. № 958 15.08.2017", Caption = "Втулка рабочего цилиндра Сборочный чертеж", InventoryNumber = 202, Name = "68-014-014 Сб", Company = TestCompanies[2], Document = TestDocuments[0], PageCount = 1, PageFormat = "А1", Person = TestPeople[1]},
            new() { Applicabilities = [TestApplicabilities[1]], Notes = "Аннулирован, замещен инв. № 880 21.12.2016", Caption = "Втулка рабочего цилиндра", InventoryNumber = 203, Name = "68-014-134", Company = TestCompanies[0], Document = TestDocuments[2], PageCount = 1, PageFormat = "А1х4", Person = TestPeople[01]},
            new() { Applicabilities = [TestApplicabilities[1]], Notes = "Аннулирован, замещен инв. № 920 27.06.2017", Caption = "Кольцо уплотнительное", InventoryNumber = 204, Name = "47А-014-028", Company = TestCompanies[0], Document = TestDocuments[2], PageCount = 1, PageFormat = "А4", Person = TestPeople[1]}
        ];
        context.Originals.AddRange(TestOriginals);

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
    public async Task GetApplicabilityCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.GetApplicabilityAsync(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("Русский дизель", res.Data.Description);
    }
    [Fact]
    public async Task GetApplicabilityListCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.GetApplicabilityListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(5, res.Data.Count);
    }
    [Fact]
    public async Task GetApplicabilityListByOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.GetApplicabilityListByOriginal(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Single(res.Data);
    }
    [Fact]
    public async Task GetFreeApplicabilityListCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.GetFreeApplicabilityList(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(4, res.Data.Count);
    }
    [Fact]
    public async Task CheckApplicability()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.CheckApplicability("М500");

        //Assert
        Assert.False(res.IsSuccess);
    }
    [Fact]
    public async Task AddApplicabilityToOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.AddOriginalToApplicability(2, 3);

        //Assert
        Assert.True(res.IsSuccess);

    }
    [Fact]
    public async Task DeleteApplicabilityFromOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.DeleteOriginalFromApplicability(2, 8);

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task CreateApplicabilityCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);
        ApplicabilityDto new_dto = new()
        {
            Description = "Д100",
            Id = 0,
        };

        //Act
        var res = await applicabilityRepo.UpsertApplicability(new_dto);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.NotEqual(0, res.Data.Id);

    }
    [Fact]
    public async Task UpdateApplicabilityCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);
        ApplicabilityDto new_dto = new()
        {
            Description = "М50",
            Id = 4,
        };

        //Act
        var res = await applicabilityRepo.UpsertApplicability(new_dto);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("М50", res.Data.Description);
    }
    [Fact]
    public async Task DeleteApplicabilityCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var applicabilityRepo = new ApplicabilityRepo(context);

        //Act
        var res = await applicabilityRepo.DeleteApplicability(5);

        //Assert
        Assert.True(res.IsSuccess);
    }
}
