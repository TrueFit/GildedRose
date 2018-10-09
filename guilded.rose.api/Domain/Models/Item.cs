using System;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Models
{
    public class Item : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Category Category { get; set; }
    }
}