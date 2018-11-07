using System;

namespace GildedRose.Entities.Inventory
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }
    }
}
