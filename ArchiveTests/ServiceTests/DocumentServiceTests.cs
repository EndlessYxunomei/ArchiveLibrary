using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using NSubstitute;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class DocumentServiceTests
{
    [Fact]
    public async Task GetTypeDocumentListTest()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        List<DocumentListDto> test_list =
            [
                new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
                new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
                new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
                new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
                new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
            ];
        documentRepo.GetDocumentListAsync(Arg.Any<ArchiveModels.DocumentType>())
            .Returns(x => Result<List<DocumentListDto>>.Success(test_list.FindAll(y => y.DocumentType == (ArchiveModels.DocumentType)x[0])));
        //documentRepo.GetDocumentListAsync(Arg.Is<ArchiveModels.DocumentType>(x => x == ArchiveModels.DocumentType.AddOriginal))
        //    .Returns(Result<List<DocumentListDto>>.Success(test_list.FindAll(y => y.DocumentType == ArchiveModels.DocumentType.AddOriginal)));
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.GetDocumentListAsync(ArchiveModels.DocumentType.AddOriginal);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(2,res.Data.Count);
    }
    [Fact]
    public async Task GetDocumentListTest()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        List<DocumentListDto> test_list =
            [
                new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
                new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
                new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
                new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
                new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
            ];
        documentRepo.GetDocumentListAsync()
            .Returns(Result<List<DocumentListDto>>.Success(test_list));
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.GetDocumentListAsync();

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(5, res.Data.Count);
    }
    [Fact]
    public async Task GetDocumentDetailCorrectly()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        List<DocumentDetailDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
       documentRepo.GetDocumentDetailAsync(Arg.Any<int>()).Returns(y => Result<DocumentDetailDto>.Success(test_list.First(x => x.Id == (int)y[0])));
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.GetDocumentDetailAsync(3);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(ArchiveModels.DocumentType.CreateCopy, res.Data.DocumentType);
    }
    [Fact]
    public async Task CheckDocumentIsNotExists()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        documentRepo.CheckDocument(Arg.Any<string>(), default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.CheckDocument("test", new DateOnly(2000,12,31));

        //Assert
        Assert.True(res.IsSuccess);
    }
    [Fact]
    public async Task UpsertDocumentCorrectly()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        List<DocumentListDto> test_list =
        [
            new() { Id = 1, Name = "test1", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 2, Name = "test2", DocumentType = ArchiveModels.DocumentType.AddOriginal },
            new() { Id = 3, Name = "test3", DocumentType = ArchiveModels.DocumentType.CreateCopy },
            new() { Id = 4, Name = "test4", DocumentType = ArchiveModels.DocumentType.DeliverCopy },
            new() { Id = 5, Name = "test5", DocumentType = ArchiveModels.DocumentType.AddCorrection }
        ];
        documentRepo.UpsertDocument(Arg.Any<DocumentDetailDto>()).ReturnsForAnyArgs(Result<int>.Success(2));
        documentRepo.GetDocumentAsync(Arg.Any<int>()).Returns(x => Result<DocumentListDto>.Success(test_list.First(y => y.Id == (int)x[0])));
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.UpsertDocument(new() { Id = 2, DocumentType = ArchiveModels.DocumentType.CreateCopy, Name = "test"});

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal("test2", res.Data.Name);
    }
    [Fact]
    public async Task DeleteDocumentCorrectly()
    {
        //Arrange
        var documentRepo = Substitute.For<IDocumentRepo>();
        documentRepo.DeleteDocument(default).ReturnsForAnyArgs(Result<Nothing>.Success());
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.DeleteDocument(1);

        //Assert
        Assert.True(res.IsSuccess);
    }
}
