using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class ApplicabilityDto : IIdentityModel
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public int OriginalId { get; set; }

    public static explicit operator ApplicabilityDto(Applicability applicability)
    {
        return new ApplicabilityDto()
        {
            Id = applicability.Id,
            Description = applicability.Description
        };
    }
}
