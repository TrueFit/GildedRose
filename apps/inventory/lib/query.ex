defmodule Inventory.Query do
  @moduledoc """
  Read the inventory.
  """
  import Inventory.Projection
  import Inventory.EventStore.Reader
  alias Inventory.Projection.AllStreams
  alias Inventory.Projection.ItemDetails

  @doc """
  Get details on a single item in inventory.
  """
  @spec item_details(String.t) :: ItemDetails.t
  def item_details(item_id) do
    item_id
    |> stream_item()
    |> Enum.reduce(%ItemDetails{}, &(project(&2, &1)))
  end

  def inventory(name \\ "*", status \\ "*") do
    name = String.downcase(name)
    status = String.downcase(status)

    stream_all_items()
    |> AllStreams.projection()
    |> Enum.filter(fn i -> name == "*" or i.name == name end)
    |> Enum.filter(fn i -> status != "trash" or i.quality == 0 end)
  end
end
