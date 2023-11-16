using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTrial1.BS
{
    public interface IBusinessContextValidation
    {
        void validateInsert();
        void validateUpdate();
        void validateDelete();
    }
}
