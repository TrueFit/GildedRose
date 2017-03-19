defmodule Inventory.Projection.ItemDetails do
  @moduledoc false
  @type t :: %Inventory.Projection.ItemDetails{}

  alias Inventory.Projection.ItemDetails
  alias Inventory.Domain.Item
  alias Inventory.Event

  defstruct item_id: "", version: 0, name: "", category: "", sell_in: 0, quality: 0

  defimpl Inventory.Projection do
    @spec project(ItemDetails.t, %EventStore.EventData{}) :: ItemDetails.t
    def project(p, %EventStore.RecordedEvent{data: event, metadata: %{"item_id" => id}, stream_version: v}) do
      %ItemDetails{p | item_id: id, version: v}
      |> set_event(event)
    end

    @spec set_event(ItemDetails.t, struct) :: ItemDetails.t
    defp set_event(p, %Event.ItemAdded{name: n, category: c, sell_in: s, quality: q}) do
      %ItemDetails{p | name: n, category: c, sell_in: s, quality: q}
    end
    defp set_event(%ItemDetails{name: n, category: c, sell_in: s, quality: q} = p, %Event.DayPassed{}) do
      {_, _, s!, q!} = Item.age({n, c, s, q})

      %ItemDetails{p | sell_in: s!, quality: q!}
    end
    defp set_event(p, %Event.ItemNameChanged{name: n}), do: %Inventory.Projection.ItemDetails{p | name: n}
    defp set_event(p, _), do: p
  end
end
