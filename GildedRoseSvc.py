import argparse
import dill as pickle
import os

class InventoryObject:
    """ Class to represent a generic Gilded Rose inventory object

    Manages object attributes such as when it needs to sell by and it's current quality value
    Object attributes update at the end of the day, so the EndDay method is the key method that modifies object attributes
    Rather than a complex hierarchy of subclasses of special cases, objects with special sellIn or quality modifiers pass in
     custom update functions to run in EndDay
    """

    def __init__(self, objName, objCategory, objSellIn, objQuality, customUpdateSellInFunc=None, customUpdateQualityFunc=None, objMaxQuality=50):
        """Inventory object constructor

        objName - the name of the object
        objCategory - the category the object belongs to
        objSellIn - number of days left in which to sell the item before it expires
        objQuality - initial object quality value
        customUpdateSellInFunc - (default None) if specified, a custom function to use to update the sellIn value at the end of the day
                                 if not specified, sellIn day decreases by 1 every day
        customUpdateQualityFunc - (default None) if specified, a custom function to use to update the quality value at the end of the day
                                  if not specified, quality decreases by 1 every day, and by 2 after sellIn < 0
        objMaxQuality - (default 50) maximum quality value for an item. 
        """

        self.__name = objName
        self.__category = objCategory
        self.__sellIn = objSellIn
        self.__maxQuality = objMaxQuality
        self.__quality = self.__SetQuality(objQuality)

        if customUpdateSellInFunc is not None:
            self.__updateSellIn = customUpdateSellInFunc
        else:
            self.__updateSellIn = lambda s : s - 1

        if customUpdateQualityFunc is not None:
            self.__updateQuality = customUpdateQualityFunc
        else:
            self.__updateQuality = lambda s, q : q - 1 if s >= 0 else q - 2

    def Name(self):
        return self.__name

    def Category(self):
        return self.__category

    def SellIn(self):
        return self.__sellIn

    def Quality(self):
        return self.__quality
    
    def __SetQuality(self, q):
        """Bounds checking for update item quality value

        q - updated quality value
        Returns corrected quality value within bounds
        """
        
        if q > self.__maxQuality:
            # Max quality is typically 50, but in some special cases it's not. In any case, quality is never higher than the max value for the item
            return self.__maxQuality
        elif  q < 0:
            # quality can never be less than 0
            return 0
        else:
            return q
    
    def EndDay(self):
        """End of day accounting

        Updates sellIn and quality values
        """
        
        # decrement sellIn first, so that quality starts degrading faster at end of day 0
        self.__sellIn = self.__updateSellIn(self.__sellIn)

        self.__quality = self.__SetQuality(self.__updateQuality(self.__sellIn, self.__quality))
        if (self.__category.lower() == 'conjured'):
            # Items of type Conjured degrade 2x as fast, so run their daily update quality func a second time
            # TODO: For Conjured items that increase quality as they age, it will increase 2x as fast. Is that right?
            self.__quality = self.__SetQuality(self.__updateQuality(self.__sellIn, self.__quality))


    def Print(self):
        """Prints an inventory object"""
        print('Name: ' + self.__name)
        print('Category: ' + self.__category)
        print('Sell In: ' + str(self.__sellIn))
        print('Quality: ' + str(self.__quality))
        print()

class InventoryObjectFactory:
    
    def Create(inventoryLine):
        """Creates inventory objects, with customizations as required by special objects

        inventoryLine - CSV string with object name, category, sell-in value, quality
        """
    
        inventoryLine.strip()
        toks = inventoryLine.split(',')

        # Validation
        if len(toks) != 4:
            raise ValueError('Invalid inventory format. Wrong number of args. Expected 4. ' + str(len(toks)) + ' given.')
        name, category, sellIn, quality = toks
        try:
            x = int(sellIn)
        except:
            raise ValueError('Invalid inventory format. SellIn must be digit')
        try:
            x = int(quality)
        except ValueError:
            raise ValueError('Invalid inventory format. Quality must be digit')

        # Handle special cases
        if name.lower() == 'aged brie':
            # Aged brie quality increases the older it gets
            # TODO: Does this mean quality increases by 2 after sellIn date?
            return InventoryObject(name, category, int(sellIn), int(quality), customUpdateQualityFunc=lambda s, q: q + 1 if s >= 0 else q + 2)
        elif category.lower() == 'sulfuras':
            # Sulfuras is a legendary item. It never has to be sold. It's quality is 80 and never changes.
            return InventoryObject(name, category, int(sellIn), 80, customUpdateSellInFunc=lambda s : s, customUpdateQualityFunc=lambda s, q: q, objMaxQuality=80)
        elif category.lower() == 'backstage passes':
            # Backstage passes quality increases as SellIn day approaches. Quality increases by 2 when there are 10 days or less, and by 3 when
            #  there are 5 days or less. Quality is 0 after the concert.
            def BackstagePassesQualityUpdateFunc(s, q):
                if s < 0:
                    return 0
                elif s <= 5:
                    return q + 3
                elif s <= 10:
                    return q + 2
                else:
                    return q + 1
            return InventoryObject(name, category, int(sellIn), int(quality), customUpdateQualityFunc=BackstagePassesQualityUpdateFunc)
        else:
            return InventoryObject(name, category, int(sellIn), int(quality))
    
class DataPresenter:
    """Manages interactions with data model"""
    
    def __init__(self, dataModel):
        """Constructor

        dataModel - list of InventoryObjects
        """
        self.__model = dataModel

    def GetAllInventory(self):
        """Returns all InventoryObjects"""
        return self.__model

    def GetInventoryByName(self, name):
        """Returns all InventoryObjects that match name"""
        return [x for x in self.__model if x.Name() == name]

    def EndDay(self):
        """Runs the EndDay method for all InventoryObjects"""
        [x.EndDay() for x in self.__model]

    def GetTrash(self):
        """Returns all InventoryObjects with quality less than 0"""
        return [x for x in self.__model if x.Quality() == 0]

    def AddItem(self, *args):
        """Adds an item to the inventory"""
        s = ','
        self.__model.append(InventoryObjectFactory.Create(s.join(args)))
        
    def RemoveItem(self, name):
        """Removes an item from the inventory"""
        self.__model = [x for x in self.__model if x.Name() != name]
        
class CLIMenu:
    """Command line interface to inventory manager"""
    
    def __init__(self, dataPresenter):
        """Constructor

        dataPresenter - Presenter componenet to interact with data model
        """
        
        self.__presenter = dataPresenter
        self.__done = False
        self.__menuChoices = {1 : self.__ShowAll,
                              2 : self.__LookupByName,
                              3 : self.__EndDay,
                              4 : self.__ShowTrash,
                              5 : self.__AddItem,
                              6 : self.__RemoveItem,
                              7 : self.__Exit
                              }                  
        
    def Exec(self):
        """Exec loop for CLI"""
        while not self.__done:
            self.__DisplayPrompt()
            choice = self.__GetInput()
            if not choice:
                continue
            self.__ProcessInput(choice)
        print('Goodbye')

    def __DisplayPrompt(self):
        print()
        print('>>> Gilded Rose Inventory Manager <<<')
        print()
        print('1: Show all inventory')
        print('2: Lookup inventory by name')
        print('3: End day')
        print('4: Show trash')
        print('5: Add item')
        print('6: Remove item')
        print('7: Exit')

    def __GetInput(self):
        try:
            return int(input('Choice: '))
        except ValueError:
            print('Invalid choice')

    def __ProcessInput(self, ch):
        if ch in self.__menuChoices:
            self.__menuChoices[ch]()
        else:
            print('Invalid choice')

    def __ShowAll(self):
        [i.Print() for i in self.__presenter.GetAllInventory()]

    def __LookupByName(self):
        name = input('Object name: ')
        [i.Print() for i in self.__presenter.GetInventoryByName(name)]

    def __EndDay(self):
        self.__presenter.EndDay()

    def __ShowTrash(self):
        [i.Print() for i in self.__presenter.GetTrash()]

    def __AddItem(self):
        name = input('Object name: ')
        category = input('Object category: ')
        sellIn = input('Sell in: ')
        quality = input('Quality: ')
        try:
            self.__presenter.AddItem(name, category, sellIn, quality)
        except ValueError as e:
            print('Error adding object. ' + str(e))
            
    def __RemoveItem(self):
        name = input('Object name: ')
        self.__presenter.RemoveItem(name)
        
    def __Exit(self):
        self.__done = True

class DataLoader:
    """Loads data model either from persisted storage from a previous session or from a file"""

    STORAGE_FILE = 'inventory.bin'
    
    def LoadFromFile(filename):
        """Drops any previous session info and loads from file"""
        DataLoader.__DropOldDatabase()
        model = []
        try:
            with open(filename) as f:
                lineNum = 0
                for line in f:
                    lineNum += 1
                    try:
                        model.append(InventoryObjectFactory.Create(line))
                    except ValueError as e:
                        print('Error in ' + filename + ' line ' + str(lineNum) + '. ' + str(e))
        except FileNotFoundError:
            print('File ' + filename + ' not found.')
        return model

    def LoadFromStorage():
        model = []
        try:
            with open(DataLoader.STORAGE_FILE, 'rb') as f:
                model = pickle.load(f)
        except FileNotFoundError:
            model = []
        return model

    def SaveToStorage(model):
        with open(DataLoader.STORAGE_FILE, 'wb') as f:
            pickle.dump(model, f)
            
    def __DropOldDatabase():
        try:
            os.remove(DataLoader.STORAGE_FILE)
        except FileNotFoundError:
            pass

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Gilded Rose inventory manager')
    parser.add_argument('-f', '--file', required=False, help='Drop any saved inventory and reload from file.')
    args = parser.parse_args()
    if args.file:
        print('Loading from file' + args.file)
        presenter = DataPresenter(DataLoader.LoadFromFile(args.file))
    else:
        presenter = DataPresenter(DataLoader.LoadFromStorage())
    cli = CLIMenu(presenter)
    cli.Exec()
    DataLoader.SaveToStorage(presenter.GetAllInventory())

    
