defmodule Inventory.Projection.ItemDetails do
  @moduledoc false
  @type t :: %__MODULE__{}

  alias Inventory.Domain.Item
  alias Inventory.Event

  defstruct item_id: "", version: 0, name: "", category: "", sell_in: 0, quality: 0, valid: true

  @doc """
  Convert a stream of events into details on an item in inventory.
  """
  def projection(stream), do: projection(stream, %__MODULE__{})
  def projection(stream, initial) do
    stream
    |> Enum.reduce(initial, &project/2)
  end

  @spec project(%EventStore.EventData{}, __MODULE__.t) :: __MODULE__.t
  defp project(%EventStore.RecordedEvent{data: event, metadata: %{"item_id" => id}, stream_version: v}, p) do
    set_event(event, %__MODULE__{p | item_id: id, version: v})
  end

  @spec set_event(struct, __MODULE__.t) :: __MODULE__.t
  defp set_event(%Event.ItemAdded{name: n, category: c, sell_in: s, quality: q}, p) do
    %__MODULE__{p | name: n, category: c, sell_in: s, quality: q}
  end
  defp set_event(%Event.InvalidItemAdded{name: n, category: c, sell_in: s, quality: q}, p) do
    %__MODULE__{p | name: n, category: c, sell_in: s, quality: q, valid: false}
  end
  defp set_event(%Event.FailedAddingFromFile{}, p), do: p
  defp set_event(_event, %__MODULE__{valid: false} = item), do: item
  defp set_event(%Event.DayPassed{}, p) do
    {_, _, s!, q!} = Item.age({p.name, p.category, p.sell_in, p.quality})

    %__MODULE__{p | sell_in: s!, quality: q!}
  end
  defp set_event(%Event.ItemNameChanged{name: n}, p), do: %__MODULE__{p | name: n}
end
