using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Membership.Models
{
    public class Jwt
    {
        public string Issuer { get; set; }

        public string Key { get; set; }
    }
}
