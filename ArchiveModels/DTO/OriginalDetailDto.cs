using ArchiveModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ArchiveModels.DTO;

public class OriginalDetailDto : IIdentityModel
{
    public int Id { get; set; }
    [Range(1, int.MaxValue)]
    public required int InventoryNumber { get; set; }
    [StringLength(ArchiveConstants.MAX_NAME_LENGTH)]
    [MinLength(1)]
    public required string Name { get; set; } = string.Empty;
    [StringLength(ArchiveConstants.MAX_ORIGINAL_CAPTION_LENGTH)]
    [MinLength(1)]
    public required string Caption { get; set; } = string.Empty;
    [StringLength(ArchiveConstants.MAX_ORIGINAL_PAGES_FORMAT_LENGTH)]
    public string? PageFormat { get; set; }
    [Range(1, int.MaxValue)]
    public int PageCount { get; set; }
    public CompanyDto? Company { get; set; }
    public DocumentListDto? Document { get; set; }
    [StringLength(ArchiveConstants.MAX_ORIGINAL_NOTES_LENGTH)]
    public string? Notes { get; set; }
    public PersonListDto? Person { get; set; }
    //список копий
    public List<CopyListDto> Copies { get; set; } = [];
    //список корекций
    public List<CorrectionListDto> Corrections { get; set; } = [];
    //список применяемости
    public List<ApplicabilityDto> Applicabilities { get; set; } = [];

    public static explicit operator OriginalDetailDto(Original original)
    {
        List<Copy> copyList = original.Copies;
        List<CopyListDto> copyDtos = [];
        copyDtos.AddRange(copyList.Select(copy => (CopyListDto)copy));

        List<Correction> correctionList = original.Corrections;
        List<CorrectionListDto> corDtos = [];
        corDtos.AddRange(correctionList.Select(cor => (CorrectionListDto)cor));

        List<Applicability> applicList = original.Applicabilities;
        List<ApplicabilityDto> appDtos = [];
        appDtos.AddRange(applicList.Select(apps => (ApplicabilityDto)apps));

        return new OriginalDetailDto()
        {
            Id = original.Id,
            InventoryNumber = original.InventoryNumber,
            Name = original.Name,
            Caption = original.Caption,
            PageFormat = original.PageFormat,
            PageCount = original.PageCount,
            Notes = original.Notes,
            Company = original.Company != null ? (CompanyDto)original.Company : null,
            Document = original.Document != null ? (DocumentListDto)original.Document : null,
            Person = original.Person != null ? (PersonListDto)original.Person : null,
            Copies = copyDtos,
            Corrections = corDtos,
            Applicabilities = appDtos
        };
    }
}
