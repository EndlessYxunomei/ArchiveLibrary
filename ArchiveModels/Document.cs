﻿using System.ComponentModel.DataAnnotations;

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
}
public enum DocumentType
{
    AddOriginal,
    CreateCopy,
    DeleteCopy,
    DeliverCopy,
    AddCorrection
}
