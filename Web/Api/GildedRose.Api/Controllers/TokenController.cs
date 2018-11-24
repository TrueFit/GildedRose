using GildedRose.Api.Models;
using GildedRose.Api.Validators;
using GildedRose.Membership;
using GildedRose.Membership.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GildedRose.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IdentityHelper auth;
        private CreateAccount_Validator test;

        public TokenController(
            IdentityHelper auth,
            CreateAccount_Validator test)
        {
            this.auth = auth;
            this.test = test;
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
            var result = await this.test.ValidateAsync(newAccount);
            if (!result.IsValid)
            {
                var errorResponse = new List<ResponseError>();
                foreach (var error in result.Errors)
                {
                    errorResponse.Add(
                        new ResponseError()
                        {
                            Field = error.PropertyName,
                            ErrorMessage = error.ErrorMessage,
                            InputData = error.AttemptedValue,
                        });
                }

                return this.BadRequest(errorResponse);
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