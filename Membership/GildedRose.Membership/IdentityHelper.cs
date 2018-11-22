using GildedRose.Core.Contracts;
using GildedRose.Membership.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GildedRose.Membership
{
    public class IdentityHelper
    {
        private IConfigurationStore config;

        public IdentityHelper(IConfigurationStore config)
        {
            this.config = config;
        }

        public string BuildToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetConfiguration<string>("Jwt:Key")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.config.GetConfiguration<string>("Jwt:Issuer"),
                this.config.GetConfiguration<string>("Jwt:Issuer"),
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com", Birthdate = new DateTime(1983, 9, 23) };
            }

            if (login.Username == "mary" && login.Password == "barbie")
            {
                user = new UserModel { Name = "Mary Smith", Email = "mary.smith@domain.com", Birthdate = new DateTime(2001, 5, 13) };
            }

            return user;
        }
    }
}
