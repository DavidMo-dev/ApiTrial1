using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ApiTrial1.BS;
using ApiTrial1.Data.Entities;
using ApiTrial1.Commons.Helpers;

namespace ApiTrial1.BL.ADM
{
    public class BL_ADM_User : BusinessContext<ADM_User>
    {

        #region Getters

        public override IQueryable<ADM_User> getAll()
        {
            return base.getAll().Where(p => p.Deleted == null || p.Deleted == false).AsQueryable();
        }

        public ADM_User getById(int Id)
        {
            return getAll().Where(p => p.Id == Id).SingleOrDefault();
        }

        public ADM_User getByUsername(string Username)
        {
            return getAll().Where(p => p.Username == Username).FirstOrDefault();
        }

        //compare the user's session token to the last stored session token
        public ADM_User getByUsernameAndToken(string Username, string Token)
        {
            return getAll().Where(p => p.Username == Username && p.ADM_User_Access.LastOrDefault().Token == Token).FirstOrDefault();
        }


        #endregion

        #region Insert

        public override void insert(ADM_User objeto)
        {
            if (objeto.Deleted == null)
            {
                objeto.Deleted = false;
            }
            base.insert(objeto);
        }

        #endregion

        #region Delete

        public override void delete(ADM_User objeto)
        {
            objeto.Deleted = true;
        }

        #endregion

        #region Password Hash

        public string getPasswordHash(string Username, string Password)
        {
            var result = SHA256Helper.SHA256($"{Username}{BS.BS.configuration.GetValue<string>("UserHashToken")}{Password}");
            return result;
        }

        #endregion

    }
}
