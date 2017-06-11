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
    |> Enum.filter(build_name_filter(name))
    |> Enum.filter(build_status_filter(status))
  end

  defp build_name_filter("*"), do: fn _ -> true end
  defp build_name_filter(f), do: fn x -> x.name |> String.downcase() |> String.contains?(f) end

  defp build_status_filter("*"), do: fn _ -> true end
  defp build_status_filter("valid"), do: fn x -> x.valid end
  defp build_status_filter("trash"), do: fn x -> x.valid and x.quality == 0 end
  defp build_status_filter("invalid"), do: fn x -> not x.valid end
  defp build_status_filter(_), do: fn _ -> false end
end
