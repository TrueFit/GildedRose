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
using GildedRose.Membership.Crypto;
using System.Security.Cryptography;
using System.Security.Claims;

namespace GildedRose.Membership
{
    public class IdentityHelper
    {
        private IConfigurationStore config;
        private UserDbContext dbContext;
        private PasswordHasher hasher;

        public IdentityHelper(
            IConfigurationStore config,
            UserDbContext dbContext,
            PasswordHasher hasher)
        {
            this.config = config;
            this.dbContext = dbContext;
            this.hasher = hasher;
        }

        public string BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
            var key = this.config.GetConfiguration<string>("Secret");
            var iv = this.config.GetConfiguration<string>("IV");

            var userAccount = this.dbContext
                .Users
                .Where(x => x.UserName.ToUpper() == login.Username.ToUpper())
                .FirstOrDefault();

            if (userAccount == null)
            {
                return null;
            }

            string inputPassword = login.Password;

            //take the salt out of the string
            byte[] retrievedHashBytes = Convert.FromBase64String(this.hasher.DecryptPassword(key, iv, userAccount.PasswordHash));
            byte[] retrievedSalt = new byte[32];
            Array.Copy(retrievedHashBytes, 0, retrievedSalt, 0, 32);

            //hash the user inputted PW with the salt
            var retrieved_pbkdf2 = new Rfc2898DeriveBytes(inputPassword, retrievedSalt, 10000);
            byte[] retrievedHash = retrieved_pbkdf2.GetBytes(20);

            //compare restuls! byte by byte!
            //starting from 16 in the stored array cause 0-15 are the salt there.
            int ok = 1;
            for (int i = 0; i < 20; i++)
            {
                if (retrievedHashBytes[i + 32] != retrievedHash[i])
                {
                    ok = 0;
                }
            }

            // if there are no differences between the strings, grant access
            if (ok != 1)
            {
                return null;
            }

            return userAccount;
        }

        public async Task<int> CreateAccount(CreateAccountModel newAccount)
        {
            var connection = this.dbContext.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            var key = this.config.GetConfiguration<string>("Secret");
            var iv = this.config.GetConfiguration<string>("IV");

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[32]);

            //var username = newAccount.UserName;
            var password = newAccount.Password;
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            //place the string in the byte array
            byte[] hash = pbkdf2.GetBytes(20);

            //make new byte array where to store the hashed password+salt
            //20 are for the hash and 16 for the salt
            byte[] hashBytes = new byte[52];

            //place the hash and salt in their respective places
            Array.Copy(salt, 0, hashBytes, 0, 32);
            Array.Copy(hash, 0, hashBytes, 32, 20);

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
                        PasswordHash = this.hasher.EncryptPassword(key, iv, Convert.ToBase64String(hashBytes)),
                        Organization = newAccount.OrganizationIdentifier,
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex) when (ex.Message.Contains("duplicate key"))
            {
                return 0;
            }
        }
    }
}
