using GildedRose.Core.Contracts;
using GildedRose.Membership.Data;
using GildedRose.Membership.Entities;
using GildedRose.Membership.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dapper;

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

        public string BuildToken(User user)
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

            var tokenTimeout = int.Parse(this.config.GetConfiguration<string>("TokenTimeout") ?? "30");
            var token = new JwtSecurityToken(
                this.config.GetConfiguration<string>("Jwt:Issuer"),
                this.config.GetConfiguration<string>("Jwt:Issuer"),
                claims,
                expires: DateTime.Now.AddMinutes(tokenTimeout),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Authenticate(LoginModel login)
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

        public async Task<int> CreateAccount(CreateAccountModel newAccount)
        {
            var connection = this.dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            try
            {
                return await connection.ExecuteScalarAsync<int>(
                    "[membership].[CreateAccount]",
                    new
                    {
                        newAccount.UserName,
                        newAccount.FirstName,
                        newAccount.LastName,
                        newAccount.Email,
                        PasswordHash = newAccount.Password,
                        Organization = newAccount.OrganizationIdentifier,
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex) when (ex.Message.Contains("duplicate key"))
            {
                return 0;
            }
        }

        public async Task<User> GetUser(int id)
        {
            return await this.dbContext
                .Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
