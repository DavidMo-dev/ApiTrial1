using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTrial1.Commons.Result;
using ApiTrial1.Commons.Request;
using ApiTrial1.Data.Entities;
using System.Diagnostics.Eventing.Reader;

namespace ApiTrial1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserControler : ControllerBase
    {

        [ActionName("setcandidate")]
        [HttpPost]
        public ActionResult SetCandidate(SetCandidateRquest request)
        {
            try
            {
                var bs = new BS.BS();
                var user = bs.ADM_User.getByUsernameAndToken(request.Username, request.Token);

                //if the user is null, the token could not be matched, session was expired.
                if (user == null)
                {
                    return ResultClass.WithError("Session expired.");
                }//check if current loged user has a recruiter role
                else if (user.ADM_Role.Id == 100)
                {

                    var candidate = bs.ADM_User.getById(request.CandidateId);

                    if (candidate == null)
                    {
                        //creates new user with a candidate role
                        candidate = new ADM_User();
                        candidate.Username = request.CandidateUsername;
                        candidate.PasswordHash = bs.ADM_User.getPasswordHash(request.CandidateUsername, request.CandidatePassword);
                        candidate.RoleId = 200;

                        bs.ADM_User.insert(candidate);

                    }//checks if user to be modified has a candidate role
                    else if (candidate.ADM_Role.Id != 200)
                    {
                        return ResultClass.WithError("This user is not a candidate.");
                    }
                    else 
                    {
                        //Edits existing candidate
                        candidate.Username = request.CandidateUsername;
                        candidate.PasswordHash = bs.ADM_User.getPasswordHash(request.CandidateUsername, request.CandidatePassword);
                       
                    }

                    //apply changes or candidate insert on save(), validations donde in BS class
                    bs.save();
                    
                    return ResultClass.WithContent(new GenericResult
                    {
                        Response = true
                    });
                }
                else{
                    return ResultClass.WithError("Access denied.");
                }
            }
            catch
            {
                return ResultClass.WithError("ERR-CODE");
            }
        }

        [ActionName("deletecandidate")]
        [HttpPost]
        public ActionResult DeleteCandidate(SetCandidateRquest request)
        {
            try
            {
                var bs = new BS.BS();
                var user = bs.ADM_User.getByUsernameAndToken(request.Username, request.Token);

                //if the user is null, the token could not be matched, session was expired.
                if (user == null)
                {
                    return ResultClass.WithError("Session expired.");
                }
                //check if current loged user has a recruiter role
                else if (user.ADM_Role.Id == 100)
                {
                    var candidate = bs.ADM_User.getById(request.CandidateId);

                    if (candidate == null)
                    {

                        return ResultClass.WithError("User not found.");

                    }
                    //checks if user to be deleted has a candidate role
                    else if (candidate.ADM_Role.Id != 200)
                    {
                        return ResultClass.WithError("This user is not a candidate.");
                    }
                    else
                    {
                        bs.ADM_User.delete(candidate);
                    }

                    //apply deletion, validations donde in BS class
                    bs.save();

                    return ResultClass.WithContent(new GenericResult
                    {
                        Response = true
                    });
                }
                else
                {
                    return ResultClass.WithError("Access denied.");
                }

            }
            catch
            {
                return ResultClass.WithError("ERR-CODE");
            }

        }


    }
}
