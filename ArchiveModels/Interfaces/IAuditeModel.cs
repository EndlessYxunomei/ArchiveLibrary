namespace ArchiveModels.Interfaces;

public interface IAuditeModel
{
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
