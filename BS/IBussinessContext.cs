using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTrial1.Data.Model;

namespace ApiTrial1.BS
{
    public interface IBusinessContext
    {
        BS bs { get; set; }
        ApiTrialDbContext db { get; set; }
    }
}
