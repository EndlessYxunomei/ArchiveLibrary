using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using NSubstitute;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class CompanyServiceTests
{
    [Fact]
    public async Task GetCompanyListTest()
    {
        //Arrange
        var companyRepo = Substitute.For<ICompanyRepo>();
        List <CompanyDto> test_list =
            [
                new() { Id = 1, Name = "test1"},
                new() { Id = 2, Name = "test2", Description = "test_description"}
            ];
        companyRepo.GetCompanyListAsync().Returns(Result<List<CompanyDto>>.Success(test_list));
        var companyService = new CompanyService(companyRepo);

        //Act
        var res = await companyService.GetCompanyListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test_description", res.Data[1].Description);
    }
    [Fact]
    public async Task GetCompanyDtoCorrectly()
    {
        //Arrange
        var companyRepo = Substitute.For<ICompanyRepo>();
        List<CompanyDto> test_list =
            [
                new() { Id = 1, Name = "test1"},
                new() { Id = 2, Name = "test2", Description = "test_description"}
            ];
        companyRepo.GetCompanyAsync(2).Returns(Result<CompanyDto>.Success(test_list[1]));
        var companyService = new CompanyService(companyRepo);

        //Act
        var res = await companyService.GetCompanyAsync(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test_description", res.Data.Description);
    }
    [Fact]
    public async Task CheckCompanyIsNotExists()
    {
        //Arrange
        var companyRepo = Substitute.For<ICompanyRepo>();
        companyRepo.CheckCompany(Arg.Any<string>()).Returns(Result<Nothing>.Success());
        var companyService = new CompanyService(companyRepo);

        //Act
        var res = await companyService.CheckCompany("test");

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task UpsetCopmpanyCorrectly()
    {
        //Arrange
        var companyRepo = Substitute.For<ICompanyRepo>();
        List<CompanyDto> test_list =
            [
                new() { Id = 1, Name = "test1"},
                new() { Id = 2, Name = "test2", Description = "test_description"}
            ];
        companyRepo.UpsertCompany(Arg.Any<CompanyDto>()).Returns(Result<CompanyDto>.Success(test_list[1]));
        var companyService = new CompanyService(companyRepo);

        //Act
        var res = await companyService.UpsertCompany(new() { Id = 0, Name = "test"});

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test_description", res.Data.Description);
    }
    [Fact]
    public async Task DeleteCompanyCorretly()
    {
        //Arrange
        var companyRepo = Substitute.For<ICompanyRepo>();
        companyRepo.DeleteCompany(default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var companyService = new CompanyService(companyRepo);

        //Act
        var res = await companyService.DeleteCompany(5);

        //Assert
        Assert.True(res.IsSuccess);
    }
}
