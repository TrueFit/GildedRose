defmodule Inventory.Projection.ItemDetails do
  @moduledoc false
  @type t :: %Inventory.Projection.ItemDetails{}

  defstruct item_id: "", version: 0, name: "", category: "", sell_in: 0, quality: 0

  defimpl Inventory.Projection do
    @spec project(Inventory.Projection.ItemDetails.t, %EventStore.EventData{}) :: Inventory.Projection.ItemDetails.t
    def project(p, %EventStore.RecordedEvent{data: event, metadata: %{"item_id" => id}, stream_version: v}) do
      %Inventory.Projection.ItemDetails{p | item_id: id, version: v}
      |> set_event(event)
    end

    @spec set_event(Inventory.Projection.ItemDetails.t, struct) :: Inventory.Projection.ItemDetails.t
    defp set_event(p, %Inventory.Event.ItemAdded{name: n, category: c, sell_in: s, quality: q}) do
      %Inventory.Projection.ItemDetails{p | name: n, category: c, sell_in: s, quality: q}
    end
    defp set_event(p, %Inventory.Event.ItemNameChanged{name: n}), do: %Inventory.Projection.ItemDetails{p | name: n}
    defp set_event(p, %Inventory.Event.DayPassed{}), do: process_day(p)
    defp set_event(p, _), do: p


    defp process_day(%Inventory.Projection.ItemDetails{category: "sulfuras"} = p), do: p
  end
end
