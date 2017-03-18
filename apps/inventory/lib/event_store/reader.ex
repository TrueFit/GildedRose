defmodule Inventory.EventStore.Reader do
  @moduledoc false

  @spec stream_item(String.t) :: Enumerable.t
  def stream_item(item_id), do: EventStore.stream_forward(item_id)

  @spec stream_all_items() :: Enumerable.t
  def stream_all_items, do: EventStore.stream_all_forward()
end
