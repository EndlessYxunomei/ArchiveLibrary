using System.ComponentModel.DataAnnotations.Schema;

namespace ArchiveModels;

public class Delivery : FullAuditableModel
{
    public virtual List<Copy> Copies { get; set; } = [];
    public int PersonId { get; set; }
    public virtual Person? Person { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public int? DeliveryDocumentId { get; set; }
    [ForeignKey("DeliveryDocumentId")]
    public virtual Document? DeliveryDocument { get; set; }
    public DateOnly ReturnDate { get; set; }
    public int? ReturnDocumentId { get; set; }
    [ForeignKey("ReturnDocumentId")]
    public virtual Document? ReturnDocument { get; set; }
}
