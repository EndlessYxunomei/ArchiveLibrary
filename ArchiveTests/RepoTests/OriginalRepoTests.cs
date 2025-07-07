using ArchiveDB;
using ArchiveModels;
using ArchiveModels.DTO;
using DataLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ArchiveTests.RepoTests;

public class OriginalRepoTests: IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ArchiveDbContext> _contextOptions;
    #region ConstructorAndDispose
    public OriginalRepoTests()
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
            new() { Description = "Д42"}
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
        List<Copy> TestCopies =
        [
            new() { Original = TestOriginals[0], CreationDocument = TestDocuments[3], CopyNumber = 1},
            new() { Original = TestOriginals[0], CreationDocument = TestDocuments[3], CopyNumber = 2},
            new() { Original = TestOriginals[0], CreationDocument = TestDocuments[4], CopyNumber = 3},
            new() { Original = TestOriginals[4], CreationDocument = TestDocuments[4], CopyNumber = 1},
            new() { Original = TestOriginals[4], CreationDocument = TestDocuments[5], CopyNumber = 2},
            new() { Original = TestOriginals[1], CreationDocument = TestDocuments[5], CopyNumber = 1, DeletionDocument = TestDocuments[9], DelitionDate = DateOnly.Parse("2005.01.12")}
        ];
        context.Copies.AddRange(TestCopies);
        List<Delivery> TestDeliveries =
        [
            new() { DeliveryDocument = TestDocuments[6], DeliveryDate = DateOnly.MinValue, Person = TestPeople[2], Copies = [TestCopies[0], TestCopies[1]] },
            new() { DeliveryDocument = TestDocuments[7], DeliveryDate = DateOnly.MinValue, Person = TestPeople[2], Copies = [TestCopies[2]] },
            new() { DeliveryDocument = TestDocuments[8], DeliveryDate = DateOnly.MinValue, Person = TestPeople[3], Copies = [TestCopies[3]] },
        ];
        context.Deliveries.AddRange(TestDeliveries);
        List<Correction> TestCorrections =
        [
            new() { Original = TestOriginals[0], Description = "Исправлен материал", CorrectionNumber = 1, Document = TestDocuments[10] },
            new() { Original = TestOriginals[1], Description = "Добавлен недостающий лист", CorrectionNumber = 1, Document = TestDocuments[11] },
            new() { Original = TestOriginals[0], Description = "Исправлена шероховатость", CorrectionNumber = 2, Document = TestDocuments[10] },
        ];
        context.Corrections.AddRange(TestCorrections);

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
    public async Task OriginalListCreatedCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var test_original_list = await originalRepo.GetOriginalList();

        //Assert
        Assert.True(test_original_list.IsSuccess);
        Assert.Equal(1236, test_original_list.Data.Last().OriginalInventoryNumber);
    }
    
    [Fact]
    public async Task OriginalDtoCreatedCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var test_original_dto = await originalRepo.GetOriginalAsync(1);
        var inventory_number = await context.Originals.FirstAsync(s => s.Id == 1);

        //Assert
        Assert.True(test_original_dto.IsSuccess);
        Assert.Equal(inventory_number.InventoryNumber, test_original_dto.Data.OriginalInventoryNumber);
    }
    [Fact]
    public async Task OriginalDetailDtoCreatedCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var test_original_dto = await originalRepo.GetOriginalDetailAsync(1);
        var inventory_number = await context.Originals.FirstAsync(s => s.Id == 1);

        //Assert
        Assert.True(test_original_dto.IsSuccess);
        Assert.Equal(inventory_number.InventoryNumber, test_original_dto.Data.InventoryNumber);
    }

    [Fact]
    public async Task GetLastInventoryNumber()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var test_number = await originalRepo.GetLastInventoryNumberAsync();

        //Assert
        Assert.True(test_number.IsSuccess);
        Assert.Equal(1236, test_number.Data);
    }
    [Fact]
    public async Task CheckFreeInventoryNumber()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var test_number = await originalRepo.CheckInventoryNumberAsync(1000);

        //Assert
        Assert.True(test_number.IsSuccess);
    }

    [Fact]
    public async Task CreateOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);
        OriginalDetailDto new_original = new()
        {
            Caption = "Test_original",
            InventoryNumber = 2000,
            Name = "test_create",
        };

        //Act
        var result = await originalRepo.UpsertOriginal(new_original);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(0,result.Data);
    }
    [Fact]
    public async Task UpdateOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);
        OriginalDetailDto updated_original = new()
        {
            Id = 5,
            Caption = "Updated_original",
            InventoryNumber = 666,
            Name = "test_update",
        };

        //Act
        var result = await originalRepo.UpsertOriginal(updated_original);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5,result.Data);
    }
    [Fact]
    public async Task DeleteOriginalCorrectly()
    {
        //Arrange
        using var context = CreateContext();
        var originalRepo = new OriginalRepo(context);

        //Act
        var result = await originalRepo.DeleteOriginal(1);

        //Assert
        Assert.True(result.IsSuccess);
    }
}
