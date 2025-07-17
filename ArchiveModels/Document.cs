using ArchiveModels.DTO;
using System.ComponentModel.DataAnnotations;

namespace ArchiveModels;

public class Document : FullAuditableModel
{
    //Обозначение документа
    public required string Name { get; set; }
    //Описание документа
    [StringLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
    public string? Description { get; set; }
    //Дата документа (отличается от даты создания записи)
    public DateOnly Date { get; set; }
    //Компания, выпустившая документ
    public int CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    //Тип документа
    public DocumentType DocumentType { get; set; }

    public static explicit operator Document(DocumentDetailDto dto)
    {
        return new Document()
        { 
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Date = new DateOnly(dto.Date.Year, dto.Date.Month, dto.Date.Day),
            CompanyId = dto.Company!.Id,
            DocumentType = dto.DocumentType
        };
    }
}
public enum DocumentType
{
    AddOriginal,
    CreateCopy,
    DeleteCopy,
    DeliverCopy,
    AddCorrection
}
