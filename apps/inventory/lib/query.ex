defmodule Inventory.Query do
  @moduledoc """
  Read the inventory.
  """

  @doc """
  Get details on a single item in inventory.
  """
  @spec item_details(String.t) :: Inventory.Projection.ItemDetails.t
  def item_details(item_id) do
    item_id
    |> Inventory.EventStore.Reader.stream_item()
    |> Enum.reduce(%Inventory.Projection.ItemDetails{}, &project/2)
  end

  @doc """
  Get every item in inventory.
  """
  def inventory() do
  end

  defp project(x, acc), do: Inventory.Projection.project(acc, x)
end
