using ApiTrial1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTrial1.Data.Model
{
    public class ApiTrialDbContext : DbContext
    {
        public ApiTrialDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ADM_User> ADM_Users { get; set; }
        public DbSet<ADM_Role> ADM_Roles { get; set; }
        public DbSet<ADM_User_Access> ADM_User_Accesses { get; set; }
        public DbSet<DCM_Document> DCM_Documents { get; set; }

    }
}
