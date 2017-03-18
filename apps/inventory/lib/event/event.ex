defmodule Inventory.Event do
  @moduledoc false
  alias Inventory.Event, as: Event

  @spec to_type_string(struct) :: String.t
  def to_type_string(%Event.ItemAdded{}), do: "ItemAdded"
  def to_type_string(%Event.ItemNameChanged{}), do: "ItemNameChanged"

  @spec to_struct(String.t) :: struct
  def to_struct("ItemAdded"), do: %Event.ItemAdded{}
  def to_struct("ItemNameChanged"), do: %Event.ItemNameChanged{}
end
