using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class CompanyDto : IIdentityModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public static explicit operator CompanyDto(Company company)
    {
        return new CompanyDto()
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
        };
    }
}
