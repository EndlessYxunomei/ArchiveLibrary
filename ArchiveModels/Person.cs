using ArchiveModels.DTO;
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

    public static explicit operator Person(PersonDetailDto dto)
    {
        return new Person()
        { 
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Department = dto.Department
        };
    }
}
