using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTrial1.BS
{
    public interface IBusinessContextPostEvents
    {
        void addInsertEntities();
        void addUpdateEntities();
        void addDeleteEntities();

        void afterInsert();
        void afterUpdate();
        void afterDelete();
    }
}
