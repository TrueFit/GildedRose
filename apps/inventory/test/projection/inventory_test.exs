defmodule Inventory.Projection.InventoryTest do
  use ExUnit.Case

  test "builds inventory" do
    # GIVEN
    stream = [
      item_added_event("item 1", "item 1", "armor", 20, 30),
      item_added_event("item 2", "item 2", "food", 16, 35),
      day_passed_event("item 1", 2),
      day_passed_event("item 2", 2)
    ]
    
    # WHEN
    result = Inventory.Projection.Inventory.projection(stream)
    
    # THEN
    assert (length result) == 2
    
    item1 = Enum.at(result, 0)
    assert item1.version == 2
    assert item1.name == "item 1"
    assert item1.sell_in == 19
    assert item1.quality == 29

  end

  defp item_added_event(id, n, c, s, q) do
    %EventStore.RecordedEvent{
      metadata: %{"item_id" => id},
      data: %Inventory.Event.ItemAdded{name: n, category: c, sell_in: s, quality: q},
      stream_version: 1
    }
  end

  defp day_passed_event(id, ver) do
    %EventStore.RecordedEvent{
      metadata: %{"item_id" => id},
      data: %Inventory.Event.DayPassed{},
      stream_version: ver
    }
  end

end
