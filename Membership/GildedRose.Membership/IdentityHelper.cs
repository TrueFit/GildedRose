using GildedRose.Core.Contracts;
using GildedRose.Membership.Data;
using GildedRose.Membership.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace GildedRose.Membership
{
    public class IdentityHelper
    {
        private IConfigurationStore config;
        private UserDbContext dbContext;

        public IdentityHelper(
            IConfigurationStore config,
            UserDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }

        public string BuildToken(UserModel user)
        {
            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return null;
            }

            return this.dbContext
                .Users
                .Where(x => x.UserName.ToUpper() == login.Username.ToUpper() && x.PasswordHash == login.Password)
                .FirstOrDefault();
        }
    }
}
