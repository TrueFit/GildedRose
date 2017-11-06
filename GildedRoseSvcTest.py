import unittest
from GildedRoseSvc import DataPresenter, InventoryObject, InventoryObjectFactory

class InventoryObjectTestCase(unittest.TestCase):

    def test_DefaultEndDay(self):
        invobj = InventoryObject('name', 'category', 5, 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 4)

    def test_ExtraQualityDecreaseAfterSellInDayPasses(self):
        invobj = InventoryObject('name', 'category', 0, 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), -1)
        self.assertEqual(invobj.Quality(), 3)

    def test_ExtraQualityDecreaseAfterSellInDayPasses(self):
        invobj = InventoryObject('name', 'category', 0, 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), -1)
        self.assertEqual(invobj.Quality(), 3)
        
    def test_QualityNeverLessThanZero(self):
        invobj = InventoryObject('name', 'category', 0, 0)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), -1)
        self.assertEqual(invobj.Quality(), 0)

    def test_ConjuredDegradesTwiceAsFast(self):
        invobj = InventoryObject('name', 'conjured', 5, 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 3)

    def test_CustomSellInFunc(self):
        invobj = InventoryObject('name', 'category', 5, 5, customUpdateSellInFunc=lambda s : s * 2)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 10)
        self.assertEqual(invobj.Quality(), 4)

    def test_CustomQualityFunc(self):
        invobj = InventoryObject('name', 'category', 5, 5, customUpdateQualityFunc=lambda s, q : q * 2)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 10)

    def test_QualityNeverGreaterThanMax(self):
        invobj = InventoryObject('name', 'category', 5, 5, customUpdateQualityFunc=lambda s, q : q * 1000, objMaxQuality=20)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 20)

class InventoryObjectFactoryTestCase(unittest.TestCase):

    def test_CreateInventoryObject(self):
        invobj = InventoryObjectFactory.Create('name,category,5,5')
        self.assertEqual(invobj.Name(), 'name')
        self.assertEqual(invobj.Category(), 'category')
        self.assertEqual(invobj.SellIn(), 5)
        self.assertEqual(invobj.Quality(), 5)

    def test_CreateInventoryObjectBadData(self):
        with self.assertRaises(ValueError):
            InventoryObjectFactory.Create('name,category,5')

    def test_CreateAgedBrie(self):
        invobj = InventoryObjectFactory.Create('Aged Brie,category,5,5')
        self.assertEqual(invobj.Name(), 'Aged Brie')
        self.assertEqual(invobj.Category(), 'category')
        self.assertEqual(invobj.SellIn(), 5)
        self.assertEqual(invobj.Quality(), 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 6)

    def test_CreateSulfuras(self):
        invobj = InventoryObjectFactory.Create('name,Sulfuras,5,80')
        self.assertEqual(invobj.Name(), 'name')
        self.assertEqual(invobj.Category(), 'Sulfuras')
        self.assertEqual(invobj.SellIn(), 5)
        self.assertEqual(invobj.Quality(), 80)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 5)
        self.assertEqual(invobj.Quality(), 80)

    def test_CreateBackstagePasses(self):
        invobj = InventoryObjectFactory.Create('name,Backstage Passes,5,5')
        self.assertEqual(invobj.Name(), 'name')
        self.assertEqual(invobj.Category(), 'Backstage Passes')
        self.assertEqual(invobj.SellIn(), 5)
        self.assertEqual(invobj.Quality(), 5)
        invobj.EndDay()
        self.assertEqual(invobj.SellIn(), 4)
        self.assertEqual(invobj.Quality(), 8)

class DataPresenterTestCase(unittest.TestCase):

    def setUp(self):
        self.model = []
        self.model.append(InventoryObjectFactory.Create('obj1,cat1,5,5'))
        self.model.append(InventoryObjectFactory.Create('obj2,cat2,5,5'))
        self.model.append(InventoryObjectFactory.Create('obj3,cat3,5,0'))
        self.d = DataPresenter(self.model)
        
    def test_GetAllInventory(self):
        m = self.d.GetAllInventory()
        self.assertEqual(len(m), 3)
        self.assertEqual(m[0].Name(), 'obj1')
        self.assertEqual(m[1].Name(), 'obj2')
        self.assertEqual(m[2].Name(), 'obj3')

    def test_GetInventoryByName(self):
        m = self.d.GetInventoryByName('obj2')
        self.assertEqual(len(m), 1)
        self.assertEqual(m[0].Name(), 'obj2')

    def test_EndDay(self):
        self.d.EndDay()
        m = self.d.GetAllInventory()
        self.assertEqual(len(m), 3)
        self.assertEqual(m[0].Quality(), 4)

    def test_GetTrash(self):
        m = self.d.GetTrash()
        self.assertEqual(len(m), 1)
        self.assertEqual(m[0].Name(), 'obj3')

    def test_AddItem(self):
        self.d.AddItem('obj4', 'cat4', '5', '5')
        m = self.d.GetAllInventory()
        self.assertEqual(len(m), 4)
        self.assertEqual(m[3].Name(), 'obj4')

    def test_RemoveItem(self):
        self.d.RemoveItem('obj2')
        m = self.d.GetAllInventory()
        self.assertEqual(len(m), 2)
        
if __name__ == '__main__':
    unittest.main()
