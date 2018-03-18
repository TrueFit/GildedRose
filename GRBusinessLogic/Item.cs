using System;

namespace GR.BusinessLogic {
    public class Item   {
        private string name;
        private string category;
        private int sellIn;
        private int quality;
        
        public Item(){
        }

        // Public properties
        public string Name{
            get{ return this.name; }
            set { this.name = value; }
        }
        public string Category {
            get { return this.category; }
            set { this.category = value;}
        }
        public int SellIn {
            get { return this.sellIn; }
            set { this.sellIn = value; }
        }
        public int Quality {
            get { return this.quality; }
            set { this.quality = value; }
        }
    }
}
