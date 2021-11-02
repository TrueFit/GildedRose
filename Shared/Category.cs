using System;

namespace GildedRose_Blazor.Shared
{
    //Defines the Category data object
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsLegendary { get; set; }
        public int DegenerationFactor { get; set; }
    }
}
