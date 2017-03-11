defmodule InventoryItemEventCreatorTest do
  use ExUnit.Case
  doctest InventoryItemEventCreator

  test "add item to inventory will error for invalid name" do
    {:error, :invalid_name} = InventoryItemEventCreator.item_added_to_inventory("", :food, 10, 10)
  end

  test "add item to inventory will error for invalid category" do
    {:error, :invalid_category} = InventoryItemEventCreator.item_added_to_inventory("banana", :bad, 10, 10)
  end

  test "add item to inventory will error for string sell_in" do
    {:error, :invalid_sell_in} = InventoryItemEventCreator.item_added_to_inventory("banana", :food, "abc", 10)
  end

  test "add item to inventory will error for negative quality" do
    {:error, :invalid_quality} = InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, -10)
  end

  test "add item to inventory will error for excessive quality" do
    {:error, :invalid_quality} = InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, 80)
  end

  test "Given an item with category sulfuras and quality 40 when the item is attempted to be added to inventory then a malformed error occurs" do
    {:error, :invalid_quality} = InventoryItemEventCreator.item_added_to_inventory("awesome", :sulfuras, 10, 150)
  end

  test "add item to inventory will create item added to inventory event" do
    {:ok, event} = InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, 10)

    assert(event.name == "banana")
    assert(event.category == :food)
    assert(event.sell_in == 10)
    assert(event.quality == 10)
  end

  test "Given a sulfuras item with quality 80 when the item is attempted to be added to inventory then an item added to inventory event is raised" do
    {:ok, event} = InventoryItemEventCreator.item_added_to_inventory("awesome", :sulfuras, 10, 80)

    assert(event.name == "awesome")
    assert(event.category == :sulfuras)
    assert(event.sell_in == 10)
    assert(event.quality == 80)
  end
end
