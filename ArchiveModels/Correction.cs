
namespace ArchiveModels;

public class Correction : FullAuditableModel
{
    //К какому оригиналу отностися
    public int OriginalId { get; set; }
    public virtual Original? Original { get; set; }
    //номер изменения
    public int CorrectionNumber { get; set; }
    //Обоснование изменения
    public int DocumentId { get; set; }
    public virtual Document? Document { get; set; }
    //Описание изменений
    public required string Description { get; set; }

}
