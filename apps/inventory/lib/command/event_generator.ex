defmodule Inventory.EventGenerator do
  @moduledoc false
  import ItemValidation
  alias Inventory.Event.ItemAdded, as: Added
  alias Inventory.Event.ItemNameChanged, as: NameChanged

  @doc """
  Create an ItemAdded event if provided with valid input.

  ## Example
      iex> Inventory.EventGenerator.item_added_to_inventory("", :food, 10, 10)
      {:error, :invalid_name}

      iex> Inventory.EventGenerator.item_added_to_inventory("banana", :bad, 10, 10)
      {:error, :invalid_category}

      iex> Inventory.EventGenerator.item_added_to_inventory("banana", :food, "abc", 10)
      {:error, :invalid_sell_in}

      iex> Inventory.EventGenerator.item_added_to_inventory("banana", :food, 10, -10)
      {:error, :invalid_quality}

      iex> Inventory.EventGenerator.item_added_to_inventory("banana", :food, 10, 80)
      {:error, :invalid_quality}

      iex> Inventory.EventGenerator.item_added_to_inventory("awesome", :sulfuras, 10, 150)
      {:error, :invalid_quality}

      iex> Inventory.EventGenerator.item_added_to_inventory("banana", :food, 10, 10)
      {:ok, %Inventory.Event.ItemAdded{category: :food, name: "banana", quality: 10, sell_in: 10}}

      iex> Inventory.EventGenerator.item_added_to_inventory("awesome", :sulfuras, 10, 80)
      {:ok, %Inventory.Event.ItemAdded{category: :sulfuras, name: "awesome", quality: 80, sell_in: 10}}
  """
  @spec item_added_to_inventory(String.t, atom, integer, integer) :: {:ok, Added.t} | {:error, atom}
  def item_added_to_inventory(name, category, sell_in, quality) do
    with {:ok, n} <- validate_name(name),
         {:ok, c} <- validate_category(category),
         {:ok, s} <- validate_sell_in(sell_in),
         {:ok, q} <- validate_quality(c, quality),
     do: {:ok, %Added{name: n, category: c, sell_in: s, quality: q}}
  end

  @doc """
  Create an ItemNameChanged event if provided with valid input.

  ## Example
      iex> Inventory.EventGenerator.item_name_changed("")
      {:error, :invalid_name}

      iex> Inventory.EventGenerator.item_name_changed("Spear")
      {:ok, %Inventory.Event.ItemNameChanged{name: "Spear"}}
  """
  @spec item_name_changed(String.t) :: {:ok, NameChanged.t} | {:error, atom}
  def item_name_changed(new_name) do
    with {:ok, n} <- validate_name(new_name),
    do: {:ok, %NameChanged{name: n}}
  end

end
