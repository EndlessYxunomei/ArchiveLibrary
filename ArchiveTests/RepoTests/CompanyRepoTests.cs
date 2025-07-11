﻿using ArchiveDB;
using ArchiveModels.DTO;
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
    public class CompanyRepoTests: IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ArchiveDbContext> _contextOptions;

        #region ConstructorAndDispose
        public CompanyRepoTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<ArchiveDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new ArchiveDbContext(_contextOptions);

            context.Companies.AddRange(
                new() { Name = "СКБД", Description = "Русский дизель" },
                new() { Name = "завод \"Русский дизель\"" },
                new() { Name = "ПО \"Звезда\"" },
                new() { Name = "ОАО \"КЗ\"" },
                new() { Name = "п/я А-7703", Description = "Звезда" },
                new() { Name = "ВМФ" },
                new() { Name = "Гипрорыбфлот Ленинград" },
                new() { Name = "51 ЦКТИС" },
                new() { Name = "Гипрорыбфлот-сервис" },
                new() { Name = "ГУП ЦКБ \"Черноморец\"" },
                new() { Name = "ПАО \"Звезда\"" },
                new() { Name = "ФГБУ \"РСТ\"" },
                new() { Name = "АО \"ПК \"Звезда\"" },
                new() { Name = "ООО \"Компания энергоремонт\"" });

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
        public async Task CompanyDtoListCreatedCorrectly()
        {
            //Arrange
            using var context = CreateContext();
            var companyRepo = new CompanyRepo(context);

            //Act
            var test_company_list = await companyRepo.GetCompanyListAsync();

            //Assert
            Assert.True(test_company_list.IsSuccess);
            Assert.Equal(14, test_company_list.Data.Count);
            Assert.Equal("СКБД", test_company_list.Data.First().Name);
        }
    }
}
