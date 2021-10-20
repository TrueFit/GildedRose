using System;
using System.Linq;
using System.Collections.Generic;

namespace GlidedRose
{
  public class GlidedRoseApp
  {
    private List<Item> _items;

    public GlidedRoseApp(List<Item> items)
    {
      _items = items;
    }

    public void ProgressDay()
    {
      foreach (Item item in _items)
      {
        if (item.Category == "Sulfuras")
        {
          // nothing needs to be changed on this item
          continue;
        }

        if (item.Name == "Aged Brie")
        {
          // Aged Brie increases in quality as time passes
          if (item.SellIn <= 0)
          {
            item.Quality += 2;
          }
          else
          {
            item.Quality++;
          }
        }
        else if (item.Category == "Backstage Passes")
        {
          // After the concert, quality of this item drops to zero
          if (item.SellIn <= 0)
          {
            item.Quality = 0;
          }
          else
          {
            // Increase quality based on time remaining to concert
            if (item.SellIn <= 5)
            {
              item.Quality += 3;
            }
            else if (item.SellIn <= 10)
            {
              item.Quality += 2;

            }
            else
            {
              item.Quality++;
            }
          }
        }
        else if (item.Category == "Conjured")
        {
          item.Quality -= 2;
        }
        else
        {
          // Normal items
          item.Quality--;
        }

        // Check Quality values to not break rules
        if (item.Quality > 50)
        {
          item.Quality = 50;
        }
        else if (item.Quality < 0)
        {
          item.Quality = 0;
        }

        // Update SellIn value
        item.SellIn--;
      }
    }

    public IEnumerable<Item> GetList()
    {
      return _items;
    }

    public Item GetItem(string name)
    {
      return _items.Find(item => item.Name == name);
    }

    public IEnumerable<Item> GetTrashedItems()
    {
      return _items.Where(o => o.Quality == 0);
    }
  }
}
