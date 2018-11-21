using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GildedRose.Data.Interfaces;
using GildedRose.Model;
using GildedRose.Services.Items;

namespace GildedRose.Services
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A simple factory. </summary>
    ///
    /// <remarks>   Two improvements: 
    ///             1.  I'd add a dictionary to cache exisitng business objects for reuse rather than   
    ///             createing new one each pass though.
    ///             2.  Use reflection to scan the this assembly so we can autoregister new types and   
    ///             the dictionary would faciltate new lookups without the nasty switch statement.
    ///             </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ItemFactory
    {
        private readonly IStoreItemRepository _itemsRepository;

        //Sword,Weapon,30,50
        //Axe,Weapon,40,50
        //Halberd,Weapon,60,40
        //Aged Brie, Food,50,10
        //Aged Milk, Food,20,20
        //Mutton,Food,10,10
        //Hand of Ragnaros,Sulfuras,80,80
        //I am Murloc,Backstage Passes,20,10
        //Raging Ogre, Backstage Pasess,10,10
        //Giant Slayer, Conjured,15,50
        //Storm Hammer, Conjured,20,50
        //Belt of Giant Strength, Conjured,20,40
        //Cheese,Food,5,5
        //Potion of Healing,Potion,10,10
        //Bag of Holding,Misc,10,50
        //TAFKAL80ETC Concert, Backstage Passes,15,20
        //Elixir of the Mongoose, Potion,5,7
        // +5 Dexterity Vest, Armor,10,20
        //Full Plate Mail,Armor,50,50
        //Wooden Shield, Armor,10,30

        public ItemFactory(IStoreItemRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public IServiceActionBase GetBusinessObject(StoreItemDto item)
        {
            IServiceActionBase businessObject=null;
            switch (item.Category)
            {
                case "Armor":
                    businessObject = new Armor(_itemsRepository);
                    break;
                case "Backstage Passes":
                    businessObject = new BackstagePasses(_itemsRepository);
                    break;
                case "Conjured":
                    businessObject = new Conjured(_itemsRepository);
                    break;
                case "Food":
                    businessObject = new Food(_itemsRepository);
                    break;
                case "Potion":
                    businessObject = new Potion(_itemsRepository);
                    break;
                case "Sulfuras":
                    businessObject = new Sulfuras(_itemsRepository);
                    break;
                case "Weapon":
                    businessObject = new Weapon(_itemsRepository);
                    break;
                default:
                    businessObject = new StandardBusinessObject(_itemsRepository);
                    break;
            }

            return businessObject;
        }
    }
}