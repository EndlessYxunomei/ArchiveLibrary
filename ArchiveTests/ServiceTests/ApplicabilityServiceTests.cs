using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using NSubstitute;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class ApplicabilityServiceTests
{
    [Fact]
    public async Task GetApplicabilityCorrectly()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        applicabilityRepo.GetApplicabilityAsync(2).Returns(Result<ApplicabilityDto>.Success(test_list[1]));
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.GetApplicabilityAsync(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data.Description);
    }
    [Fact]
    public async Task GetApplicabilityListCorrectly()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        applicabilityRepo.GetApplicabilityListAsync().Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.GetApplicabilityListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(2, res.Data.Count);
    }
    [Fact]
    public async Task GetApplicabilityLisByOriginal()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        applicabilityRepo.GetApplicabilityListByOriginal(1).Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.GetApplicabilityListByOriginal(1);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(2, res.Data.Count);
    }
    [Fact]
    public async Task GetFreeApplicabilityList()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        applicabilityRepo.GetFreeApplicabilityList(1).Returns(Result<List<ApplicabilityDto>>.Success(test_list));
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.GetFreeApplicabilityList(1);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(2, res.Data.Count);
    }
    [Fact]
    public async Task CheckApplicabilityIsNotExists()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        applicabilityRepo.CheckApplicability(Arg.Any<string>()).ReturnsForAnyArgs(Result<Nothing>.Success());
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.CheckApplicability("test");

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task UpsertApplicability()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        List<ApplicabilityDto> test_list =
        [
            new() { Id = 1, Description = "test1"},
            new() { Id = 2, Description = "test2"}
        ];
        applicabilityRepo.UpsertApplicability(Arg.Any<ApplicabilityDto>()).ReturnsForAnyArgs(Result<ApplicabilityDto>.Success(test_list[1]));
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.UpsertApplicability(new() { Id = 2, Description = "test"});

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data.Description);
    }
    [Fact]
    public async Task DeleteApplicability()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        applicabilityRepo.DeleteApplicability(default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.DeleteApplicability(5);

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task AddApplicabilityToOriginal()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        applicabilityRepo.AddOriginalToApplicability(default, default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.AddOriginalToApplicability(1,2);

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task DeleteApplicabilityFromOriginal()
    {
        //Arrange
        var applicabilityRepo = Substitute.For<IApplicabilityRepo>();
        applicabilityRepo.DeleteOriginalFromApplicability(default, default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var applicabilityService = new ApplicabilityService(applicabilityRepo);

        //Act
        var res = await applicabilityService.DeleteOriginalFromApplicability(1,2);

        //Assert
        Assert.True(res.IsSuccess);
    }
}
