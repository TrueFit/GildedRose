defmodule InventoryItemEventCreator do
  @moduledoc false
  import ItemValidation
  alias Event.ItemAddedToInventory, as: Added

  @doc """
  Create an ItemAddedToInventory event if provided with valid input.

  ## Example
      iex> InventoryItemEventCreator.item_added_to_inventory("", :food, 10, 10)
      {:error, :invalid_name}

      iex> InventoryItemEventCreator.item_added_to_inventory("banana", :bad, 10, 10)
      {:error, :invalid_category}

      iex> InventoryItemEventCreator.item_added_to_inventory("banana", :food, "abc", 10)
      {:error, :invalid_sell_in}

      iex> InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, -10)
      {:error, :invalid_quality}

      iex> InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, 80)
      {:error, :invalid_quality}

      iex> InventoryItemEventCreator.item_added_to_inventory("awesome", :sulfuras, 10, 150)
      {:error, :invalid_quality}

      iex> InventoryItemEventCreator.item_added_to_inventory("banana", :food, 10, 10)
      {:ok, %Event.ItemAddedToInventory{category: :food, name: "banana", quality: 10, sell_in: 10}}

      iex> InventoryItemEventCreator.item_added_to_inventory("awesome", :sulfuras, 10, 80)
      {:ok, %Event.ItemAddedToInventory{category: :sulfuras, name: "awesome", quality: 80, sell_in: 10}}
  """
  @spec item_added_to_inventory(String.t, atom, integer, integer) :: {:ok, Added.t} | {:error, atom}
  def item_added_to_inventory(name, category, sell_in, quality) do
    with {:ok, n} <- validate_name(name),
         {:ok, c} <- validate_category(category),
         {:ok, s} <- validate_sell_in(sell_in),
         {:ok, q} <- validate_quality(c, quality),
     do: {:ok, %Added{name: n, category: c, sell_in: s, quality: q}}
  end

end
