using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Auth.Data;

namespace Auth.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController(AuthDbContext dbContext) : ControllerBase
    {
        private readonly AuthDbContext _dbContext = dbContext;

        [Authorize]
        [HttpGet("AddClaimsSurname")]
        public async Task<ActionResult> AddClaimsSurname()
        {
            var surname = string.Empty;
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var name = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (name == null) return BadRequest();

                surname = new string(name.Reverse().ToArray());

                claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, surname));
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);
            }

            surname = User.FindFirst(ClaimTypes.Surname)?.Value;
            if (surname == null) return BadRequest();

            return Ok($"AddClaimsSurname: {surname}");
        }

        [Authorize]
        [HttpGet("RemoveClaimsSurname")]
        public async Task<ActionResult> RemoveClaimsSurname()
        {
            if (User.Identity is ClaimsIdentity claimsIdentity)
            {
                var surname = User.FindFirst(ClaimTypes.Surname);
                //if (surname == null) return BadRequest();

                if (claimsIdentity.TryRemoveClaim(surname))
                {
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                }
            }

            return Ok($"RemoveClaimsSurname");
        }

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
            var name = User.FindFirst(ClaimTypes.NameIdentifier);

            return Ok($"Authorize: {name}");
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
