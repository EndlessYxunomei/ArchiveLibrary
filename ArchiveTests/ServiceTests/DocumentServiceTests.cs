using ArchiveModels.DTO;
using ArchiveModels.Utilities;
using DataLayer.Interfaces;
using NSubstitute;
using ServiceLayer;

namespace ArchiveTests.ServiceTests;

public class DocumentServiceTests
{
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
        documentRepo.GetDocumentListAsync(Arg.Is<ArchiveModels.DocumentType>(x => x == ArchiveModels.DocumentType.AddOriginal))
            .Returns(Result<List<DocumentListDto>>.Success(test_list.FindAll(y => y.DocumentType == ArchiveModels.DocumentType.AddOriginal)));
        var documentService = new DocumentService(documentRepo);

        //Act
        var res = await documentService.GetDocumentListAsync(ArchiveModels.DocumentType.AddOriginal);

        //Assert
        Assert.True(res.IsSuccess);
        Assert.Equal(2,res.Data.Count);
    }
}
