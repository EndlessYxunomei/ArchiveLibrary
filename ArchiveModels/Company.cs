using ArchiveModels.DTO;

namespace ArchiveModels;

public class Company : FullAuditableModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public static explicit operator Company(CompanyDto dto)
    {
        return new Company()
        {
            Name = dto.Name,
            Description = dto.Description,
            Id = dto.Id
        };
    }
}
