using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTrial1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Http;
using ApiTrial1.Data.Model;
using ApiTrial1.BS;
using ApiTrial1.BL.ADM;
using ApiTrial1.BL.DCM;

namespace ApiTrial1.BS
{
    public class BS
    {

        #region Tablas
        //ADM

        public BL_ADM_User ADM_User { get; set; }
        public BL_ADM_Role ADM_Role { get; set; }
        public BL_ADM_User_Access ADM_User_Access { get; set; }

        //DCM
        public BL_DCM_Document DCM_Document { get; set; }
        #endregion


        #region Propiedades

        public static IConfiguration configuration = null;
        public List<Action> TasksAfterCommit { get; set; } = new List<Action>();

        //private ADM_Entidades _entidad;
        //public ADM_Entidades Entidad
        //{
        //    get
        //    {
        //        if (_entidad == null)
        //        {
        //            if (_user != null && _user.ADM_Entidades != null)
        //            {
        //                _entidad = _user.ADM_Entidades;
        //            }
        //        }
        //        return _entidad;
        //    }
        //    set
        //    {
        //        _entidad = value;
        //    }
        //}

        private ADM_User _user = null;
        public ADM_User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private ApiTrialDbContext _context;
        public bool UsuarioEspecifico = false;

        #endregion

        #region Constructor

        public BS(int? UserId = null)
        {
            ConfigureDatabase();
            InitializeTables();

            if (UserId != null)
            {
                UsuarioEspecifico = true;
            }

            if (UserId == null)
            {
                if (System.Web.HttpContext.Current.Items.Any(p => (string)p.Key == "AccessToken") && !String.IsNullOrEmpty(System.Web.HttpContext.Current.Items["AccessToken"]?.ToString()))
                {
                    var accessToken = System.Web.HttpContext.Current.Items["AccessToken"]?.ToString();
                   
                    var acceso = this.ADM_User_Access.getByToken(accessToken);
                    if (acceso != null)
                    {
                        this.User = acceso.ADM_User;
                    }
                }
                else
                {
                    if (System.Web.HttpContext.Current.Request.Cookies.Any(p => p.Key == "SessionToken") && !String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies["SessionToken"]))
                    {
                        var accessToken = System.Web.HttpContext.Current.Request.Cookies["SessionToken"];
                        var acceso = this.ADM_User_Access.getByToken(accessToken);
                        if (acceso != null)
                        {
                            this.User = acceso.ADM_User;
                        }
                    }
                    else
                    {
                        User = null;
                    }
                }
            }
            else
            {
                var user = this.ADM_User.getById(UserId.Value);
                if (user != null)
                {
                    this.User = user;
                }
                else
                {
                    this.User = null;
                }
            }

        }

        //public BS(string Username)
        //{
        //    ConfigureDatabase();
        //    InitializeTables();

        //    var user = this.ADM_User.getByUsername(Username);
        //    if (user != null)
        //    {
        //        this.User = user;
        //        UsuarioEspecifico = true;
        //    }
        //    else
        //    {
        //        this.User = null;
        //    }
        //}

        public BS()
        {
            ConfigureDatabase();
            InitializeTables();
        }

        private void ConfigureDatabase()
        {
            if (BS.configuration == null)
            {
                throw new Exception("No se ha establecido una configuración. Utilice BS.configuration = configuration en el fichero Startup.cs para pasar una configuración a la capa Business.");
            }

            // Creamos un nuevo contexto.
            var optionsBuilder = new DbContextOptionsBuilder<ApiTrialDbContext>();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(BS.configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ApiTrial1"));

            this._context = new ApiTrialDbContext(optionsBuilder.Options);

            this._context.Database.SetCommandTimeout(new TimeSpan(0, 2, 0));
        }

        private void InitializeTables()
        {

            // Instanciamos todas las propiedades.
            foreach (PropertyInfo Propiedad in this.GetType().GetProperties())
            {

                string[] PropiedadesExcluir = { "User", "Entidad" };
                if (!PropiedadesExcluir.Contains(Propiedad.Name))
                {
                    // Instanciamos la propiedad
                    var constructor = Propiedad.PropertyType.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        this.GetType().GetProperty(Propiedad.Name).SetValue(this, constructor.Invoke(null), null);
                    }

                    // Si la propiedad hereda de BusinessContext, actualizamos su contexto.
                    var valorPropiedad = this.GetType().GetProperty(Propiedad.Name).GetValue(this, null);
                    if (valorPropiedad is IBusinessContext)
                    {
                        ((IBusinessContext)valorPropiedad).db = _context;
                        ((IBusinessContext)valorPropiedad).bs = this;
                    }
                }
            }

        }

        #endregion

        #region Métodos

        /// <summary>
        /// Añade una acción para ejecutar tras el guardado del contexto de base de datos.
        /// </summary>
        /// <param name="action">Código a ejecutar tras el guardado de base de datos.</param>
        public void ExecuteAfterSaveChanges(Action action)
        {
            TasksAfterCommit.Add(action);
        }

        #endregion

        #region Save

        public void save()
        {
            string[] PropiedadesExcluir = { "User", "Entidad" };

            // Validamos la inserción, modificación y eliminación de elementos. En caso de haber errores, lanzamos una excepción y no guardamos los cambios.
            // Esto nos permitirá controlar casos particulares en distintas entidades.
            foreach (PropertyInfo Propiedad in this.GetType().GetProperties())
            {
                if (!PropiedadesExcluir.Contains(Propiedad.Name))
                {
                    var valorPropiedad = this.GetType().GetProperty(Propiedad.Name).GetValue(this, null);
                    if (valorPropiedad != null)
                    {
                        if (valorPropiedad is IBusinessContextValidation)
                        {
                            // Llamamos a los métodos validateInsert, validateUpdate y validateDelete
                            ((IBusinessContextValidation)valorPropiedad).validateDelete();
                            ((IBusinessContextValidation)valorPropiedad).validateInsert();
                            ((IBusinessContextValidation)valorPropiedad).validateUpdate();
                        }

                        if (valorPropiedad is IBusinessContextPostEvents)
                        {
                            // Llamamos a los métodos que almacenan las entidades insertadas, actualizadas y eliminadas para más adelante llamar a PostEvents
                            ((IBusinessContextPostEvents)valorPropiedad).addInsertEntities();
                            ((IBusinessContextPostEvents)valorPropiedad).addUpdateEntities();
                            ((IBusinessContextPostEvents)valorPropiedad).addDeleteEntities();
                        }
                    }
                }
            }

            try
            {
                _context.SaveChanges();
                // Ejecutamos acciones post-guardado y vaciamos la lista
                var tasksToRun = TasksAfterCommit;
                TasksAfterCommit = new List<Action>();
                foreach (var taskAfterCommit in tasksToRun)
                {
                    taskAfterCommit();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        throw new Exception(ex.Message + "\n" + ex.InnerException.Message + "\n" + ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        throw new Exception(ex.Message + "\n" + ex.InnerException.Message);
                    }
                }
                else
                {
                    throw;
                }
            }

            // Ejecutamos PostEvents
            foreach (PropertyInfo Propiedad in this.GetType().GetProperties())
            {
                if (!PropiedadesExcluir.Contains(Propiedad.Name))
                {
                    var valorPropiedad = this.GetType().GetProperty(Propiedad.Name).GetValue(this, null);
                    if (valorPropiedad != null)
                    {
                        if (valorPropiedad is IBusinessContextPostEvents)
                        {
                            ((IBusinessContextPostEvents)valorPropiedad).afterInsert();
                            ((IBusinessContextPostEvents)valorPropiedad).afterUpdate();
                            ((IBusinessContextPostEvents)valorPropiedad).afterDelete();
                        }
                    }
                }
            }

        }

        public void save(bool commitChanges)
        {
            // Por defecto, 1 minuto de Timeout
            this.save(commitChanges, 60);
        }

        public void save(bool commitChanges, long timeout)
        {
            // Validamos la inserción, modificación y eliminación de elementos. En caso de haber errores, lanzamos una excepción y no guardamos los cambios.
            // Esto nos permitirá controlar casos particulares en distintas entidades.
            string[] PropiedadesExcluir = { "User", "Entidad" };
            foreach (PropertyInfo Propiedad in this.GetType().GetProperties())
            {
                if (!PropiedadesExcluir.Contains(Propiedad.Name))
                {
                    var valorPropiedad = this.GetType().GetProperty(Propiedad.Name).GetValue(this, null);
                    if (valorPropiedad != null)
                    {
                        if (valorPropiedad is IBusinessContextValidation)
                        {
                            // Llamamos a los métodos validateInsert, validateUpdate y validateDelete
                            ((IBusinessContextValidation)valorPropiedad).validateDelete();
                            ((IBusinessContextValidation)valorPropiedad).validateInsert();
                            ((IBusinessContextValidation)valorPropiedad).validateUpdate();
                        }

                        if (valorPropiedad is IBusinessContextPostEvents)
                        {
                            // Llamamos a los métodos que almacenan las entidades insertadas, actualizadas o eliminadas para más adelante llamar a PostEvents
                            ((IBusinessContextPostEvents)valorPropiedad).addInsertEntities();
                            ((IBusinessContextPostEvents)valorPropiedad).addUpdateEntities();
                            ((IBusinessContextPostEvents)valorPropiedad).addDeleteEntities();
                        }
                    }
                }
            }

            try
            {

                using (var transaction = this._context.Database.BeginTransaction())
                {
                    this._context.SaveChanges();
                    if (commitChanges)
                    {
                        transaction.Commit();
                        var tasksToRun = TasksAfterCommit;
                        TasksAfterCommit = new List<Action>();
                        foreach (var taskAfterCommit in tasksToRun)
                        {
                            taskAfterCommit();
                        }
                    }
                    else
                    {
                        transaction.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                try
                {
                    _context.Database.GetDbConnection().Close();
                }
                catch { }

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        throw new Exception(ex.Message + "\n" + ex.InnerException.Message + "\n" + ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        throw new Exception(ex.Message + "\n" + ex.InnerException.Message);
                    }
                }
                else
                {
                    throw;
                }
            }

            // Ejecutamos PostEvents
            foreach (PropertyInfo Propiedad in this.GetType().GetProperties())
            {
                if (!PropiedadesExcluir.Contains(Propiedad.Name))
                {
                    var valorPropiedad = this.GetType().GetProperty(Propiedad.Name).GetValue(this, null);
                    if (valorPropiedad != null)
                    {
                        if (valorPropiedad is IBusinessContextPostEvents)
                        {
                            ((IBusinessContextPostEvents)valorPropiedad).afterInsert();
                            ((IBusinessContextPostEvents)valorPropiedad).afterUpdate();
                            ((IBusinessContextPostEvents)valorPropiedad).afterDelete();
                        }
                    }
                }
            }

        }

        #endregion

    }
}
