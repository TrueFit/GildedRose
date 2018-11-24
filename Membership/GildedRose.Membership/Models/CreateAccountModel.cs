using System;

namespace GildedRose.Membership.Models
{
    public class CreateAccountModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Guid? OrganizationIdentifier { get; set; }
    }
}
