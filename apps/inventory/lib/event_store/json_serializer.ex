defmodule Inventory.EventStore.JsonSerializer do
  @moduledoc """
  A serializer that uses the JSON format.
  """
  @behaviour EventStore.Serializer

  @doc """
  Serialize given term to JSON binary data.
  """
  @spec serialize(any) :: String.t
  def serialize(term) do
    Poison.encode!(term)
  end

  @doc """
  Deserialize given JSON binary data to the expected type.
  """
  @spec deserialize(String.t, map) :: any
  def deserialize(binary, config) do
    type = case Keyword.get(config, :type, nil) do
      nil -> []
      type -> type |> to_struct()
    end
    Poison.decode!(binary, as: type)
  end

  @doc """
  Map an event struct to its type string.
  """
  @spec to_type_string(struct) :: String.t
  def to_type_string(%Inventory.Event.ItemAdded{}), do: "ItemAdded"
  def to_type_string(%Inventory.Event.ItemNameChanged{}), do: "ItemNameChanged"
  def to_type_string(%Inventory.Event.DayPassed{}), do: "DayPassed"

  @doc """
  Map an event type string to its struct.
  """
  @spec to_struct(String.t) :: struct
  def to_struct("ItemAdded"), do: %Inventory.Event.ItemAdded{}
  def to_struct("ItemNameChanged"), do: %Inventory.Event.ItemNameChanged{}
  def to_struct("DayPassed"), do: %Inventory.Event.DayPassed{}
end
