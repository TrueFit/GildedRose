defmodule Inventory.Projection.Inventory do
  @moduledoc false

  @type t :: %__MODULE__{streams: [{String.t, struct}]}

  defstruct streams: %{}

  @doc """
  Convert a stream of events into an inventory.
  """
  def projection(stream), do: projection(stream, %__MODULE__{})
  def projection(stream, initial) do
    stream
    |> Enum.reduce(initial, &project/2)
    |> (fn x -> x.streams end).()
    |> Map.values()
  end


  defp project(%EventStore.RecordedEvent{metadata: %{"item_id" => id}, stream_version: ver, data: e}, p) do
    item = p.streams
           |> Map.get(id, %Inventory.Projection.ItemDetails{})
           |> project(e, id, ver)

    %__MODULE__{p | streams: Map.put(p.streams, id, item)}
  end

  defp project(_item, %Inventory.Event.ItemAdded{} = e, id, ver) do
    %Inventory.Projection.ItemDetails{
      item_id: id,
      version: ver,
      name: e.name,
      category: e.category,
      sell_in: e.sell_in,
      quality: e.quality
    }
  end

  defp project(_item, %Inventory.Event.InvalidItemAdded{} = e, id, ver) do
    %Inventory.Projection.ItemDetails{
      item_id: id,
      version: ver,
      name: e.name,
      category: e.category,
      sell_in: e.sell_in,
      quality: e.quality,
      valid: false
    }
  end

  # Never update invalid items. It will just cause further issues.
  defp project(%Inventory.Projection.ItemDetails{valid: false} = item, _event, _id, ver) do
    %Inventory.Projection.ItemDetails{item | version: ver}
  end

  defp project(item, %Inventory.Event.DayPassed{}, _id, ver) do
    {_, _, s, q} = Inventory.Domain.Item.age({item.name, item.category, item.sell_in, item.quality})

    %Inventory.Projection.ItemDetails{item | sell_in: s, quality: q, version: ver}
  end

end

