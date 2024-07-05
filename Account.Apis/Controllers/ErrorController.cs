using Account.Apis.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Apis.Controllers
{
    
    public class ErrorController : ApiBaseController
    {
        [HttpGet]
        public IActionResult Error(int code)
            => new ObjectResult(new ApiResponse(code));
    }
}
