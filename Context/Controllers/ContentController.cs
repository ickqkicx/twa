using Auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController(AuthDbContext dbContext) : ControllerBase
    {
        private readonly AuthDbContext _dbContext = dbContext;

        [AllowAnonymous]
        [HttpGet("AllowAnonymous")]
        public ActionResult AllowAnonymous()
        {
            
            return Ok("AllowAnonymous");
        }

        [Authorize]
        [HttpGet("Authorize")]
        public ActionResult Authorize()
        {
            
            return Ok("Authorize");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("AuthorizeAdmin")]
        public ActionResult AuthorizeAdmin()
        {
            
            return Ok("AuthorizeAdmin");
        }

        [Authorize(Roles = "user")]
        [HttpGet("AuthorizeUser")]
        public ActionResult AuthorizeUser()
        {
            
            return Ok("AuthorizeUser");
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet("AuthorizeAdminOrUser")]
        public ActionResult AuthorizeAdminOrUser()
        {

            return Ok("AuthorizeAdminOrUser");
        }

        [Authorize(Roles = "admin")]
        [Authorize(Roles = "user")]
        [HttpGet("AuthorizeAdminAndUser")]
        public ActionResult AuthorizeAdminAndUser()
        {

            return Ok("AuthorizeAdminAndUser");
        }


    }
}
