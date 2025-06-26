using ArchiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
