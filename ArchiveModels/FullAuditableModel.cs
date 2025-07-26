using ArchiveModels.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArchiveModels;

public class FullAuditableModel : IAuditeModel, IIdentityModel, ISoftDeletable
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}
