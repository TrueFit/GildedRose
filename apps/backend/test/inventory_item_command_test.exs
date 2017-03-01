defmodule InventoryItemCommandTest do
  use ExUnit.Case
  doctest InventoryItemCommand

  test "add item to inventory will error for invalid name" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("", :food, 10, 10)
  end

  test "add item to inventory will error for invalid category" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("banana", :bad, 10, 10)
  end

  test "add item to inventory will error for string sell_in" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("banana", :food, "abc", 10)
  end

  test "add item to inventory will error for negative quality" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("banana", :food, 10, -10)
  end

  test "add item to inventory will error for excessive quality" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("banana", :food, 10, 80)
  end

  test "Given an item with category sulfuras and quality 40 when the item is attempted to be added to inventory then a malformed error occurs" do
    {:error, :malformed} = InventoryItemCommand.add_item_to_inventory("awesome", :sulfuras, 10, 150)
  end

  test "add item to inventory will create item added to inventory event" do
    start_time = DateTime.utc_now
    {:ok, event} = InventoryItemCommand.add_item_to_inventory("banana", :food, 10, 10)
    end_time = DateTime.utc_now

    refute(event.domain_id == -1)
    assert(event.event_id == 0)
    :gt = DateTime.compare(start_time, event.timestamp)
    :lt = DateTime.compare(event.timestamp, end_time)

    %Event.ItemAddedToInventory{name: name, category: category, sell_in: sell_in, quality: quality} = event.payload
    assert(name == "banana")
    assert(category == :food)
    assert(sell_in == 10)
    assert(quality == 10)
  end

  test "Given a sulfuras item with quality 80 when the item is attempted to be added to inventory then an item added to inventory event is raised" do
    start_time = DateTime.utc_now
    {:ok, event} = InventoryItemCommand.add_item_to_inventory("awesome", :sulfuras, 10, 80)
    end_time = DateTime.utc_now

    refute(event.domain_id == -1)
    assert(event.event_id == 0)
    :gt = DateTime.compare(start_time, event.timestamp)
    :lt = DateTime.compare(event.timestamp, end_time)

    %Event.ItemAddedToInventory{name: name, category: category, sell_in: sell_in, quality: quality} = event.payload
    assert(name == "awesome")
    assert(category == :sulfuras)
    assert(sell_in == 10)
    assert(quality == 80)
  end

  test "Given two items when each item is added into inventory then their domain ids are different" do
    {:ok, event1} = InventoryItemCommand.add_item_to_inventory("banana", :food, 10, 20)
    {:ok, event2} = InventoryItemCommand.add_item_to_inventory("ice cream", :food, 1, 50)

    refute(event1.domain_id == event2.domain_id)
    refute(event1.domain_id == -1)
    refute(event2.domain_id == -1)
  end
end
