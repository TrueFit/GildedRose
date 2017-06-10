defmodule Inventory.Projection.ItemDetails do
  @moduledoc false
  @type t :: %__MODULE__{}

  alias Inventory.Domain.Item
  alias Inventory.Event

  defstruct item_id: "", version: 0, name: "", category: "", sell_in: 0, quality: 0

  @doc """
  Convert a stream of events into details on an item in inventory.
  """
  def projection(stream), do: projection(stream, %__MODULE__{})
  def projection(stream, initial) do
    stream
    |> Enum.reduce(initial, &(project(&2, &1)))
  end

  @spec project(__MODULE__.t, %EventStore.EventData{}) :: __MODULE__.t
  defp project(p, %EventStore.RecordedEvent{data: event, metadata: %{"item_id" => id}, stream_version: v}) do
    %__MODULE__{p | item_id: id, version: v}
    |> set_event(event)
  end

  @spec set_event(__MODULE__.t, struct) :: __MODULE__.t
  defp set_event(p, %Event.ItemAdded{name: n, category: c, sell_in: s, quality: q}) do
    %__MODULE__{p | name: n, category: c, sell_in: s, quality: q}
  end
  defp set_event(%__MODULE__{name: n, category: c, sell_in: s, quality: q} = p, %Event.DayPassed{}) do
    {_, _, s!, q!} = Item.age({n, c, s, q})

    %__MODULE__{p | sell_in: s!, quality: q!}
  end
  defp set_event(p, %Event.ItemNameChanged{name: n}), do: %__MODULE__{p | name: n}
  defp set_event(p, _), do: p
end
