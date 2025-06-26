using ArchiveModels.Interfaces;

namespace ArchiveModels.DTO;

public class PersonListDto : IIdentityModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    public static explicit operator PersonListDto(Person person)
    {
        return new PersonListDto
        {
            Id = person.Id,
            FullName = person.LastName + " " + (person.FirstName ?? "")
        };
    }
}
