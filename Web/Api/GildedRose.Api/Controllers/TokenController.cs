using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.Api.Models;
using GildedRose.Api.Validators;
using GildedRose.Membership;
using GildedRose.Membership.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core.Enrichers;

namespace GildedRose.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IdentityHelper auth;
        private CreateAccountModel_Validator validateCreateAccount;
        private LoginModel_Validator validateLogin;
        private ILogger logger;

        public TokenController(
            IdentityHelper auth,
            CreateAccountModel_Validator validateCreateAccount,
            LoginModel_Validator validateLogin,
            ILogger logger)
        {
            this.auth = auth;
            this.validateCreateAccount = validateCreateAccount;
            this.validateLogin = validateLogin;
            this.logger = logger;
        }

        [HttpPost]
        [Route("createtoken")]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            List<ResponseError> errorResponse;

            var result = await this.validateLogin.ValidateAsync(login);
            if (!result.IsValid)
            {
                errorResponse = new List<ResponseError>();
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

            IActionResult response;
            var user = await this.auth.Authenticate(login);

            if (user != null)
            {
                var tokenString = this.auth.BuildToken(user);
                response = this.Ok(new { token = tokenString });

                this.logger
                    .ForContext(
                        new[]
                        {
                            new PropertyEnricher("Token", tokenString),
                        })
                    .Information("Created token");
            }
            else
            {
                errorResponse = new List<ResponseError>();
                errorResponse.Add(
                      new ResponseError()
                      {
                          Field = "Credentials",
                          ErrorMessage = "Invalid username and password.",
                          InputData = string.Empty,
                      });
                response = this.Ok(new { Hint = "401", Errors = errorResponse });
            }

            return response;
        }

        [HttpPost]
        [Route("createaccount")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountModel newAccount)
        {
            var result = await this.validateCreateAccount.ValidateAsync(newAccount);
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

            if (await this.auth.CreateAccount(newAccount) != 0)
            {
                return this.Ok(new { message = "User Account has been created successfully." });
            }

            return this.BadRequest(new { message = "Account Could not be created, please try again later." });
        }
    }
}