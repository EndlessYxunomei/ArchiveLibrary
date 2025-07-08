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
}
