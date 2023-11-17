using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiTrial1.Commons.Classes;
using ApiTrial1.Commons.Result;
using ApiTrial1.Commons.Request;
using ApiTrial1.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiTrial1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileManager _iFileManager;
        public FileManagerController(IFileManager iFileManager)
        {
            _iFileManager = iFileManager;
        }


        [ActionName("uploadfile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile(FileUploadRequest request)
        {

            try 
            {

                var bs = new BS.BS();
                var user = bs.ADM_User.getByUsernameAndToken(request.Username, request.Token);

                if (user == null)
                {
                    return ResultClass.WithError("Session expired.");
                }

                var file = bs.DCM_Document.getById(request.FileId);

                if (file == null) 
                {
                    file = new DCM_Document();
                    file.FileName = request.FileName;
                    file.File = "Uploads\\StaticContent\\" + request.FileName;
                    file.UploadedBy = user.Id;

                    bs.DCM_Document.insert(file);
                }//checks if the uploader is either the original uloader or a recruiter.
                else if(file.UploadedBy != user.Id && user.ADM_Role.Id != 100)
                {
                    return ResultClass.WithError("Access denied.");
                }

                //save document, validation in class BS.
                bs.save();
                var result = await _iFileManager.UploadFile(request.File);
                return Ok(result);
            }
            catch 
            {
                return ResultClass.WithError("ERR-CODE");
            }
  
        }

        //[ActionName("downloadfile")]
        //[HttpGet]
        //public async Task<IActionResult> DownloadFile(string FileName)
        //{
        //    var result = await _iFileManager.DownloadFile(FileName);
        //    return File(result.Item1, result.Item2, result.Item2);
        //}
    }
}
