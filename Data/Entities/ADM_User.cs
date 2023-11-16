using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTrial1.Data.Entities
{
    public class ADM_User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Username { get; set; } 
        public string PasswordHash { get; set; } 
        public bool? Deleted { get; set; }
        public String? CreatedByLog { get; set; }
        public String? DeletedByLog { get; set; }
        public int? RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual ADM_Role ADM_Role { get; set; }
        public virtual ICollection<ADM_User_Access> ADM_User_Access { get; set; }

        public virtual ICollection<DCM_Document> DMC_Documents { get; set; }

    }
}
