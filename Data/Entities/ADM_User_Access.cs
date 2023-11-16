using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTrial1.Data.Entities
{
    public class ADM_User_Access
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(ADM_User_Id))]
        public int ADM_User_Id { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Token { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public DateTime? CreateDate { get; set; }
        public string IPAddress { get; set; }
        [ForeignKey(nameof(ADM_User_Id))]
        public virtual ADM_User ADM_User { get; set; }
    }
}
