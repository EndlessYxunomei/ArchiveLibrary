using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class CorrectionDetailDto: IIdentityModel
{
    public int Id { get; set; }
    public int OriginalId { get; set; }
    public string OriginalName { get; set; } = string.Empty;
    public string OriginalCaption { get; set; } = string.Empty;
    public int CorrectionNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public DocumentListDto? Document { get; set; }

    public static explicit operator CorrectionDetailDto(Correction correction)
    {
        return new CorrectionDetailDto()
        {
            Id = correction.Id,
            OriginalId = correction.OriginalId,
            Description = correction.Description,
            CorrectionNumber = correction.CorrectionNumber,
            Document = (DocumentListDto)correction.Document!,
            OriginalName = correction.Original!.Name,
            OriginalCaption = correction.Original!.Caption
        };
    }
}
