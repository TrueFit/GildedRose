namespace GildedRose.Inventory.Interfaces;  

public interface IInventoryList
{
    /// <summary>
    /// Gets the details for the requested item
    /// </summary>
    /// <param name="name">Name of the item (case sensitive) to retrieve details for</param>
    /// <returns>A comma delimited list of details for an item in the inventory</returns>
    public string GetItem(string name);
    /// <summary>
    /// Removes items to be thrown away from the inventory list
    /// </summary>
    /// <returns>A comma delimited list of items which are to be thrown away</returns>
    public string RemoveTrash();
    /// <summary>
    /// Gets a list of items in the inventory list
    /// </summary>
    /// <returns>A comma delimited list of details for each item still in the inventory list</returns>
    public string ToString();
    /// <summary>
    /// Progresses the inventory system to the next day
    /// </summary>
    public void EndDay();
}
