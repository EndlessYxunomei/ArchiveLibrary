using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class CopyDetailDto: IIdentityModel
{
    public required int Id { get; set; }
    public required int Number { get; set; }
    public DateTime Date { get; set; }
    public OriginalListDto? Original { get; set; }
    public DocumentListDto? CreationDocument { get; set; }
    public DocumentListDto? DeletionDocument { get; set; }
    public DateOnly? DelitionDate { get; set; }

    public static explicit operator CopyDetailDto(Copy copy)
    {
        return new CopyDetailDto()
        {
            Id = copy.Id,
            Number = copy.CopyNumber,
            Date = copy.CreatedDate,
            Original = copy.Original == null ? null : (OriginalListDto)copy.Original,
            CreationDocument = copy.CreationDocument == null ? null : (DocumentListDto)copy.CreationDocument,
            DeletionDocument = copy.DeletionDocument == null ? null : (DocumentListDto)copy.DeletionDocument,
            DelitionDate = copy.DelitionDate
        };
    }
}
