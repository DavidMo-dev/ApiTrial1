using Microsoft.AspNetCore.Mvc;

namespace ApiTrial1.Commons.Result
{

    public class ResultClass : ActionResult
    {

        public static ActionResult WithContent<T>(T _content)
        {
            var err = new ErrorClass();
            err.HasError = false;
            err.Message = null;
            return new JsonResult(new ReturnClass()
            {
                content = _content,
                Error = err
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ActionResult WithError(string message)
        {
            var err = new ErrorClass();
            err.HasError = true;
            err.Message = message;
            return new JsonResult(new ReturnClass()
            {
                content = null,
                Error = err
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static ActionResult NotAuthorized(string message)
        {
            var err = new ErrorClass();
            err.HasError = true;
            err.Message = message;
            return new JsonResult(new ReturnClass()
            {
                content = null,
                Error = err
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        public static ActionResult Forbidden(string message)
        {
            var err = new ErrorClass();
            err.HasError = true;
            err.Message = message;
            return new JsonResult(new ReturnClass()
            {
                content = null,
                Error = err
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
