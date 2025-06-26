using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class DocumentListDto : IIdentityModel
{
    public required string Name { get; set; }
    public required int Id { get; set; }
    public required DocumentType DocumentType { get; set; }
    public DateTime Date { get; set; }

    public static explicit operator DocumentListDto(Document document)
    {
        return new DocumentListDto()
        {
            DocumentType = document.DocumentType,
            Name = document.Name,
            Id = document.Id,
            Date = new DateTime(document.Date.Year, document.Date.Month, document.Date.Day)
        };
    }
}
