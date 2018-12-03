using System;
using GildedRose.Core.Contracts;

namespace GildedRose.Logic.Models
{
    public class Category : IAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
