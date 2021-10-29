using System;

namespace DataAccessLibrary
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
