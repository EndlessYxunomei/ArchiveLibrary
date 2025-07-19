using ArchiveModels.DTO;
using System.ComponentModel.DataAnnotations;

namespace ArchiveModels;

public class Applicability : FullAuditableModel
{
    //к какому документу относится
    public virtual List<Original> Originals { get; set; } = [];
    //Применяемость
    [StringLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
    public required string Description { get; set; }

    public static explicit operator Applicability(ApplicabilityDto dto)
    {
        return new Applicability()
        { 
            Description = dto.Description,
            Id = dto.Id,
        };
    }
}
