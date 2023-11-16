using ApiTrial1.BS;
using ApiTrial1.Data.Entities;
using ApiTrial1.Commons.Helpers;

namespace ApiTrial1.BL.ADM
{
    public class BL_ADM_User_Access : BusinessContext<ADM_User_Access>
    {
        #region Getters

        public ADM_User_Access getById(int Id)
        {
            return getAll().Where(p => p.Id == Id).SingleOrDefault();
        }

        public IQueryable<ADM_User_Access> getByUsuarioId(int UsuarioId)
        {
            return getAll().Where(p => p.ADM_User_Id == UsuarioId).AsQueryable();
        }

        public ADM_User_Access getByToken(string token)
        {
            return getAll().Where(p => p.Token == token).FirstOrDefault();
        }

        #endregion
    }
}
