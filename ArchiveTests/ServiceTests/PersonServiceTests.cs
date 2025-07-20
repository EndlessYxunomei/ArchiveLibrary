using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using NSubstitute;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class PersonServiceTests
{
    [Fact]
    public async Task GetPersonListTest()
    {
        //Arrange
        var personRepo = Substitute.For<IPersonRepo>();
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personRepo.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));
        var personService = new PersonService(personRepo);

        //Act
        var res = await personService.GetPersonListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data[1].FullName);
    }
    [Fact]
    public async Task GetPersonDetailTest()
    {
        //Arrange
        var personRepo = Substitute.For<IPersonRepo>();
        List<PersonDetailDto> test_list =
        [
            new() { Id = 1, LastName = "test1", FirstName = "test1_f", Department = "test_dep"},
            new() { Id = 2, LastName = "test2", FirstName = "test2_f", Department = "test_dep"}
        ];
        personRepo.GetPersonDetailAsync(Arg.Any<int>()).Returns(Result<PersonDetailDto>.Success(test_list[1]));
        var personService = new PersonService(personRepo);

        //Act
        var res = await personService.GetPersonDetailAsync(2);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data.LastName);
    }
    [Fact]
    public async Task CheckPersonTest()
    {
        //Arrange
        var personRepo = Substitute.For<IPersonRepo>();
        personRepo.CheckPersonFullName(Arg.Any<string>(), Arg.Any<string?>()).Returns(Result<Nothing>.Success());
        var personService = new PersonService(personRepo);

        //Act
        var res = await personService.CheckPersonFullName("test", "test");

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task UpsertPersonTest()
    {
        //Arrange
        var personRepo = Substitute.For<IPersonRepo>();
        List<PersonListDto> test_list =
        [
            new() { Id = 1, FullName = "test1"},
            new() { Id = 2, FullName = "test2"}
        ];
        personRepo.GetPersonAsync(default).ReturnsForAnyArgs(Result<PersonListDto>.Success(test_list[1]));
        personRepo.UpsertPerson(Arg.Any<PersonDetailDto>()).ReturnsForAnyArgs(Result<int>.Success(2));
        var personService = new PersonService(personRepo);

        //Act
        var res = await personService.UpsertPerson(new() { Id = 2, LastName = "test", FirstName = "test", Department = "test"});

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data.FullName);
    }
    [Fact]
    public async Task DeletePersonTest()
    {
        //Arrange
        var personRepo = Substitute.For<IPersonRepo>();
        personRepo.DeletePerson(default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var personService = new PersonService(personRepo);

        //Act
        var res = await personService.DeletePerson(2);

        //Assert
        Assert.True(res.IsSuccess);
    }
}
