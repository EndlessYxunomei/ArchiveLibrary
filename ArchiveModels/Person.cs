using System.ComponentModel.DataAnnotations;

namespace ArchiveModels;

public class Person : FullAuditableModel
{
    [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
    public string? FirstName { get; set; }
    [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
    public required string LastName { get; set; }
    [StringLength(ArchiveConstants.MAX_PERSON_DEPARTMENT_LENGTH)]
    public string? Department { get; set; }
}
