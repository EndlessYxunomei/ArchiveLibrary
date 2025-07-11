﻿using ArchiveDB;
using ArchiveModels;
using DataLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveTests.RepoTests
{
    public class DocumentRepoTests: IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ArchiveDbContext> _contextOptions;
        #region ConstructorAndDispose
        public DocumentRepoTests()
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
        public async Task DocumentListDtoCreatedCorrectly()
        {
            // Arrange
            using var context = CreateContext();
            var documentRepo = new DocumentRepo(context);

            //Act
            var test_document_list = await documentRepo.GetDocumentListAsync(DocumentType.AddOriginal);

            //Assert
            Assert.True(test_document_list.IsSuccess);
            Assert.Equal(3,test_document_list.Data.Count);
            Assert.Equal("132", test_document_list.Data[2].Name);
        }
    }
}
