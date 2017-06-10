defmodule Inventory.Projection.InventoryTest do
  use ExUnit.Case

  test "builds inventory" do
    # GIVEN
    stream = [
      item_added_event("item 1", "item 1", "armor", 20, 30),
      item_added_event("item 2", "item 2", "food", 16, 35)
    ]
    
    # WHEN
    result = Inventory.Projection.Inventory.projection(stream)
    
    # THEN
    assert (length result) == 2
  end

  defp item_added_event(id, n, c, s, q) do
    %EventStore.RecordedEvent{
      metadata: %{"item_id" => id},
      data: %Inventory.Event.ItemAdded{name: n, category: c, sell_in: s, quality: q}
    }
  end


end
