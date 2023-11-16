using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTrial1.Data.Entities
{
    public class DCM_Document
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string FileName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string File { get; set; }
        public int UploadedBy { get; set; }

        [ForeignKey(nameof(UploadedBy))]
        public virtual ADM_User ADM_User_Uploader { get; set; }

    }
}
