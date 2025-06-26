namespace ArchiveModels;

public class Company : FullAuditableModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
