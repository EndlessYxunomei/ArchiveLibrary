using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class PersonDetailDto: IIdentityModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Department { get; set; }

    public static explicit operator PersonDetailDto(Person person)
    {
        return new PersonDetailDto()
        {
            LastName = person.LastName,
            FirstName = person.FirstName,
            Department = person.Department,
            Id = person.Id
        };
    }

}