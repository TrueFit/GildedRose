defmodule Inventory.Query do
  @moduledoc """
  Read the inventory.
  """
  import Inventory.EventStore.Reader
  alias Inventory.Projection

  @doc """
  Get details on a single item in inventory.
  """
  @spec item_details(String.t) :: Projection.ItemDetails.t
  def item_details(item_id) do
    item_id
    |> stream_item()
    |> Projection.ItemDetails.projection()
  end

  def inventory(name \\ "*", status \\ "*") do
    name = String.downcase(name)
    status = String.downcase(status)

    stream_all_items()
    |> Projection.Inventory.projection()
    |> Enum.filter(fn i -> name == "*" or i.name == name end)
    |> Enum.filter(fn i -> status != "trash" or i.quality == 0 end)
  end
end
