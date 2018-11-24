using GildedRose.Membership;
using GildedRose.Membership.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost]
        [Route("createuser")]
        public IActionResult CreateToken([FromBody]LoginModel login)
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

        [HttpPost]
        [Route("createaccount")]
        public async Task<object> CreateAccount([FromBody]CreateAccountModel newAccount)
        {
            if (newAccount.Password != newAccount.ConfirmPassword)
            {
                this.BadRequest(new { message = "Passwords do not match." });
            }

            var id = await this.auth.CreateAccount(newAccount);
            if (id != 0)
            {
                return this.Ok(new { message = "User Account has been created successfully." });
            }

            return this.BadRequest(new { message = "Account Could not be created, please try again later." });
        }
    }
}