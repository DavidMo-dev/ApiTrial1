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
                    return ResultClass.WithError("Incorrect username or password.");
                }
                var hashedPassword = bs.ADM_User.getPasswordHash(request.Username, request.Password);
                if (hashedPassword != user.PasswordHash)
                {
                    return ResultClass.WithError("Incorrect username or password.");
                }

                // if correct, a new token is generated.
                var access = new ADM_User_Access();
                access.Token = SessionTokenHelper.GenerateToken() + SHA256Helper.SHA256(user.Id.ToString());
                access.CreateDate = DateTime.Now;

                // adds user access
                user.ADM_User_Access.Add(access);

                // save
                bs.save();

                // save token in cookie 
                Response.Cookies.Append("SessionToken", access.Token, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

                // return token
                return ResultClass.WithContent(new LoginResult
                {
                    Username = user.Username,
                    Token = access.Token
                });

            }
            catch
            {
                return ResultClass.WithError("Incorrect username or password.");
            }

        }

    }
}
