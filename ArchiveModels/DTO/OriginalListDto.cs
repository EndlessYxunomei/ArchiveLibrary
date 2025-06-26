using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class OriginalListDto : IIdentityModel
{
    public int Id { get; set; }
    public int OriginalInventoryNumber { get; set; }
    public string OriginalName { get; set; } = string.Empty;
    public string OriginalCaption { get; set; } = string.Empty;
    public DateTime OriginalDate { get; set; }
    public string? DocumentName { get; set; }
    public DateOnly? DocumentDate { get; set; }

    public static explicit operator OriginalListDto(Original original)
    {
        return new OriginalListDto()
        {
            Id = original.Id,
            OriginalInventoryNumber = original.InventoryNumber,
            OriginalName = original.Name,
            OriginalCaption = original.Caption,
            OriginalDate = original.CreatedDate,
            DocumentName = original.Document?.Name,
            DocumentDate = original.Document?.Date,
        };
    }
}
