import pytest
from app.inventory_item import InventoryItem

# fixtures here


@pytest.fixture
def inventory_item_bag_of_holding():
    return InventoryItem('Bag of Holding', 'Misc', 10, 50)


@pytest.fixture
def inventory_item_cheese():
    return InventoryItem('Cheese', 'Food', 1, 10)


@pytest.fixture
def inventory_item_aged_brie():
    return InventoryItem('Aged Brie', 'Food', 1, 10)


@pytest.fixture
def inventory_item_sulfuras():
    return InventoryItem('Hand of Ragnaros', 'Sulfuras', 80, 80)


@pytest.fixture
def inventory_item_backstage_passes():
    return InventoryItem('I am Murloc', 'Backstage Passes', 12, 10)


@pytest.fixture
def inventory_item_conjured():
    return InventoryItem('Storm Hammer', 'Conjured', 20, 50)


@pytest.fixture
def inventory_item_zero_days_quality_1():
    return InventoryItem('Special Case: days = 0, qual = 1', 'Misc', 0, 1)


# tests here


@pytest.mark.inventory_item_minimal
def test_name(inventory_item_bag_of_holding):
    assert inventory_item_bag_of_holding.item_name == 'Bag of Holding'


@pytest.mark.inventory_item_minimal
def test_category(inventory_item_bag_of_holding):
    assert inventory_item_bag_of_holding.item_category == 'Misc'


@pytest.mark.inventory_item_minimal
def test_sell_in(inventory_item_bag_of_holding):
    assert inventory_item_bag_of_holding.item_sell_in == 10


@pytest.mark.inventory_item_minimal
def test_quality(inventory_item_bag_of_holding):
    assert inventory_item_bag_of_holding.item_quality == 50


@pytest.mark.inventory_item_minimal
def test_str(inventory_item_bag_of_holding):
    expected_str = 'Bag of Holding - (Category: Misc) : Sell in 10 days : Quality 50'
    assert inventory_item_bag_of_holding.__str__() == expected_str


@pytest.mark.adjust_sell_in
def test_adjust_sell_in_1_day(inventory_item_bag_of_holding):
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_sell_in == 9


@pytest.mark.adjust_sell_in
def test_adjust_sell_in_2_day(inventory_item_bag_of_holding):
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_sell_in == 8


@pytest.mark.adjust_sell_in
def test_adjust_sell_in_3_day(inventory_item_bag_of_holding):
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_sell_in == 7


@pytest.mark.adjust_sell_in
def test_adjust_sell_in_beyond_sale_date(inventory_item_bag_of_holding):
    for index in range(0, 11):  # 10 iterations
        inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_sell_in == -1


@pytest.mark.adjust_quality
def test_adjust_quality_1_day(inventory_item_bag_of_holding):
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_quality == 49


@pytest.mark.adjust_quality
def test_adjust_quality_2_day(inventory_item_bag_of_holding):
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_quality == 48


@pytest.mark.adjust_quality
def test_adjust_quality_no_qual_below_zero1(inventory_item_bag_of_holding):
    """The Quality of an item is never negative"""
    for index in range(0, 52):  # 51 iterations
        inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_quality == 0


@pytest.mark.adjust_quality
def test_adjust_quality_no_qual_below_zero2(inventory_item_bag_of_holding):
    """The Quality of an item is never negative"""
    for index in range(0, 76):  # 75 iterations
        inventory_item_bag_of_holding.adjust_quality_at_end_of_day()
    assert inventory_item_bag_of_holding.item_quality == 0

@pytest.mark.adjust_quality
def test_adjust_quality_item_at_zero_days(inventory_item_zero_days_quality_1):
    """"Test edge case where quality calc dips below zero"""
    inventory_item_zero_days_quality_1.adjust_quality_at_end_of_day()
    assert inventory_item_zero_days_quality_1.item_quality == 0


@pytest.mark.adjust_quality
def test_adjust_quality_sell_by_passed_2x_degrade(inventory_item_cheese):
    """Once the sell by date has passed, Quality degrades twice as fast"""
    for index in range(0, 2):  # 2 days
        inventory_item_cheese.adjust_quality_at_end_of_day()
    assert inventory_item_cheese.item_quality == (10 - 1 - 2)


@pytest.mark.adjust_quality
def test_adjust_quality_sell_by_brie_increases(inventory_item_aged_brie):
    """"Aged Brie" actually increases in Quality the older it gets"""
    for index in range(0, 1):  # 1 days
        inventory_item_aged_brie.adjust_quality_at_end_of_day()
    assert inventory_item_aged_brie.item_quality == (10 + 1)


@pytest.mark.adjust_quality
def test_adjust_quality_qual_never_above_50_1(inventory_item_aged_brie):
    """The Quality of an item is never more than 50"""
    for index in range(0, 52):  # 51 days
        inventory_item_aged_brie.adjust_quality_at_end_of_day()
    assert inventory_item_aged_brie.item_quality == 50


@pytest.mark.adjust_quality
def test_adjust_quality_qual_never_above_50_2(inventory_item_aged_brie):
    """The Quality of an item is never more than 50"""
    for index in range(0, 72):  # 71 days
        inventory_item_aged_brie.adjust_quality_at_end_of_day()
    assert inventory_item_aged_brie.item_quality == 50


@pytest.mark.adjust_quality
def test_adjust_quality_sulfuras_quality(inventory_item_sulfuras):
    """'Sulfuras', being a legendary item, never has to be sold or decreases in Quality"""
    for index in range(0, 4):  # 3 days
        inventory_item_sulfuras.adjust_quality_at_end_of_day()
    assert inventory_item_sulfuras.item_quality == 80


@pytest.mark.adjust_quality
def test_adjust_quality_sulfuras_sell_in(inventory_item_sulfuras):
    """'Sulfuras', being a legendary item, never has to be sold or decreases in Quality"""
    for index in range(0, 4):  # 3 days
        inventory_item_sulfuras.adjust_quality_at_end_of_day()
    assert inventory_item_sulfuras.item_sell_in == 80


@pytest.mark.adjust_quality
def test_adjust_quality_backstage_passes_over10(inventory_item_backstage_passes):
    """"Backstage passes", like aged brie, increases in Quality as it's
     SellIn value approaches; Quality increases by 2 when there are 10 days
      or less and by 3 when there are 5 days or less but Quality drops to 0
       after the concert"""
    for index in range(0, 1):  # 1 days
        inventory_item_backstage_passes.adjust_quality_at_end_of_day()
    assert inventory_item_backstage_passes.item_quality == (10 + 1)


@pytest.mark.adjust_quality
def test_adjust_quality_backstage_passes_at_10(inventory_item_backstage_passes):
    """"Backstage passes", like aged brie, increases in Quality as it's
     SellIn value approaches; Quality increases by 2 when there are 10 days
      or less and by 3 when there are 5 days or less but Quality drops to 0
       after the concert"""
    for index in range(0, 2):  # 2 days
        inventory_item_backstage_passes.adjust_quality_at_end_of_day()
    assert inventory_item_backstage_passes.item_quality == (10 + 1 + 2)


@pytest.mark.adjust_quality
def test_adjust_quality_backstage_passes_at_6(inventory_item_backstage_passes):
    """"Backstage passes", like aged brie, increases in Quality as it's
     SellIn value approaches; Quality increases by 2 when there are 10 days
      or less and by 3 when there are 5 days or less but Quality drops to 0
       after the concert"""
    for index in range(0, 6):  # 6 days
        inventory_item_backstage_passes.adjust_quality_at_end_of_day()
    assert inventory_item_backstage_passes.item_sell_in == 6
    assert inventory_item_backstage_passes.item_quality == (10 + 1 + 2 + 2 + 2 + 2 + 2)


@pytest.mark.adjust_quality
def test_adjust_quality_backstage_passes_at_5(inventory_item_backstage_passes):
    """"Backstage passes", like aged brie, increases in Quality as it's
     SellIn value approaches; Quality increases by 2 when there are 10 days
      or less and by 3 when there are 5 days or less but Quality drops to 0
       after the concert"""
    for index in range(0, 7):  # 7 days
        inventory_item_backstage_passes.adjust_quality_at_end_of_day()
    assert inventory_item_backstage_passes.item_sell_in == 5
    assert inventory_item_backstage_passes.item_quality == (10 + 1 + 2 + 2 + 2 + 2 + 2 + 3)


@pytest.mark.adjust_quality
def test_adjust_quality_backstage_passes_at_0(inventory_item_backstage_passes):
    """"Backstage passes", like aged brie, increases in Quality as it's
     SellIn value approaches; Quality increases by 2 when there are 10 days
      or less and by 3 when there are 5 days or less but Quality drops to 0
       after the concert"""
    for index in range(0, 12):  # 12 days
        inventory_item_backstage_passes.adjust_quality_at_end_of_day()
    assert inventory_item_backstage_passes.item_sell_in == 0
    assert inventory_item_backstage_passes.item_quality == (10 + 1 + 5 * 2 + 6 * 3)


@pytest.mark.adjust_quality
def test_adjust_quality_conjured_items_1_day(inventory_item_conjured):
    """"'Conjured' items degrade in Quality twice as fast as normal items"""
    for index in range(0, 1):  # 1 days
        inventory_item_conjured.adjust_quality_at_end_of_day()
    assert inventory_item_conjured.item_quality == 48


@pytest.mark.adjust_quality
def test_adjust_quality_conjured_items_2_day(inventory_item_conjured):
    """"'Conjured' items degrade in Quality twice as fast as normal items"""
    for index in range(0, 2):  # 2 days
        inventory_item_conjured.adjust_quality_at_end_of_day()
    assert inventory_item_conjured.item_quality == 46
