using ApiTrial1.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiTrial1.Commons.Helpers;
using ApiTrial1.Commons.Result;
using ApiTrial1.Commons.Request;

namespace ApiTrial1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class loginController : ControllerBase
    {

        [ActionName("login")]
        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {

            try
            {

                var bs = new BS.BS();
                var user = bs.ADM_User.getByUsername(request.Username);
                if (user == null)
                {
                    return ResultClass.WithError("Usuario o contraseña incorrectos.");
                }
                var hashedPassword = bs.ADM_User.getPasswordHash(request.Username, request.Password);
                if (hashedPassword != user.PasswordHash)
                {
                    return ResultClass.WithError("Usuario o contraseña incorrectos.");
                }

                // Si todo es correcto, creamos un nuevo token.
                var access = new ADM_User_Access();
                access.Token = SessionTokenHelper.GenerateToken() + SHA256Helper.SHA256(user.Id.ToString());
                access.CreateDate = DateTime.Now;

                // Añadimos el acceso al usuario
                user.ADM_User_Access.Add(access);

                // Guardamos
                bs.save();

                // Establecemos el token en la Cookie de la sesión
                Response.Cookies.Append("SessionToken", access.Token, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

                // Devolvemos el token
                return ResultClass.WithContent(new LoginResult
                {
                    Username = user.Username,
                    Token = access.Token
                });

            }
            catch
            {
                return ResultClass.WithError("Usuario o contraseña incorrectos.");
            }

        }

    }
}
