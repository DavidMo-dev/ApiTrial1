using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ApiTrial1.BS;
using ApiTrial1.Data.Entities;

namespace ApiTrial1.BL.DCM
{
    public class BL_DCM_Document: BusinessContext<DCM_Document>
    {

        #region Getters
        public DCM_Document getById(int Id)
        {
            return getAll().Where(p => p.Id == Id).SingleOrDefault();
        }
        #endregion

        #region Insert
        #endregion

        #region Delete
        #endregion

    }
}
