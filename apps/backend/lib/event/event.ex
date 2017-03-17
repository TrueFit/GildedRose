defmodule Event do
  @moduledoc false

  @spec to_type_string(struct) :: String.t
  def to_type_string(%Event.ItemAddedToInventory{}), do: "ItemAddedToInventory"
  def to_type_string(%Event.ItemNameChanged{}), do: "ItemNameChanged"

  @spec to_struct(String.t) :: struct
  def to_struct("ItemAddedToInventory"), do: %Event.ItemAddedToInventory{}
  def to_struct("ItemNameChanged"), do: %Event.ItemNameChanged{}
end
