using GildedRose.Membership;
using GildedRose.Membership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IdentityHelper auth;

        public TokenController(IdentityHelper auth)
        {
            this.auth = auth;
        }

        [AllowAnonymous]
        [HttpPost]
        public object CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = this.Unauthorized();
            var user = this.auth.Authenticate(login);

            if (user != null)
            {
                var tokenString = this.auth.BuildToken(user);
                response = this.Ok(new { token = tokenString });
            }

            return response;
        }
    }
}