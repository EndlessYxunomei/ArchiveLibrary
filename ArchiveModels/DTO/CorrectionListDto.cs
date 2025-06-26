using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class CorrectionListDto : IIdentityModel
{
    public required int Id { get; set; }
    public required int Number { get; set; }
    public DateTime Date { get; set; }

    public static explicit operator CorrectionListDto(Correction correction)
    {
        return new CorrectionListDto()
        {
            Id = correction.Id,
            Number = correction.CorrectionNumber,
            Date = correction.CreatedDate
        };
    }
}
