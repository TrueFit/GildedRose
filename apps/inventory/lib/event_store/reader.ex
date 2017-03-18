defmodule Inventory.EventStore.Reader do
  @moduledoc false
  
  def stream_item(item_id), do: EventStore.stream_forward(item_id)

  def stream_all_items(), do: EventStore.stream_all_forward()
end
