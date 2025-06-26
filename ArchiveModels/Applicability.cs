using System.ComponentModel.DataAnnotations;

namespace ArchiveModels;

public class Applicability : FullAuditableModel
{
    //к какому документу относится
    public virtual List<Original> Originals { get; set; } = [];
    //Применяемость
    [StringLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
    public required string Description { get; set; }
}
