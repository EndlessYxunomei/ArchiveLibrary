﻿using ArchiveModels.DTO;
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
        var pesronRepo = Substitute.For<IPersonRepo>();
        List<PersonListDto> test_list =
            [
                new() { Id = 1, FullName = "test1"},
                new() { Id = 2, FullName = "test2"}
            ];
        pesronRepo.GetPersonListAsync().Returns(Result<List<PersonListDto>>.Success(test_list));
        var personService = new PersonService(pesronRepo);

        //Act
        var res = await personService.GetPersonListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data[1].FullName);
    }
}
