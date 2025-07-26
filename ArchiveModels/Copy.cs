using System.ComponentModel.DataAnnotations.Schema;

namespace ArchiveModels;

public class Copy : FullAuditableModel
{
    //к какому оригиналу отностится
    public int OriginalId { get; set; }
    public virtual Original? Original { get; set; }
    //номер копии (не является id)
    public int CopyNumber { get; set; }
    //Обоснование создания копии
    public int? CreationDocumentId { get; set; }
    [ForeignKey("CreationDocumentId")]
    public virtual Document? CreationDocument { get; set; }
    //обоснование уничтожения копии
    public int? DeletionDocumentId { get; set; }
    [ForeignKey("DeletionDocumentId")]
    public virtual Document? DeletionDocument { get; set; }
    //Дата уничтожения копии
    public DateOnly? DelitionDate { get; set; }
    public List<Delivery> Deliveries { get; set; } = [];
}
