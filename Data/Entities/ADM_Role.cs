using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTrial1.Data.Entities
{
    public class ADM_Role
    {
        public ADM_Role(int id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }
        //Roles creted on first execution
        //Update comment as table contents are updated.
        // Id 100 = Recruiter, Id 200 = Candidate.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public String RoleName { get; set; }

        public virtual ICollection<ADM_User> ADM_Users { get; set; }
    }
}
