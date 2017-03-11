defmodule InventoryItemEventCreator do
  @moduledoc false
  import ItemValidation
  alias Event.ItemAddedToInventory, as: Added

  @doc """
  Validate command and create an ItemAddedToInventory event.
  """
  @spec item_added_to_inventory(String.t, atom, integer, integer) :: {:ok, Added.t} | {:error, String.t}
  def item_added_to_inventory(name, category, sell_in, quality) do
    with {:ok, n} <- validate_name(name),
         {:ok, c} <- validate_category(category),
         {:ok, s} <- validate_sell_in(sell_in),
         {:ok, q} <- validate_quality(c, quality),
     do: {:ok, %Added{name: n, category: c, sell_in: s, quality: q}}
  end

end
