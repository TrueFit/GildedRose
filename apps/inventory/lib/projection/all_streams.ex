defmodule Inventory.Projection.AllStreams do
  @moduledoc false

  @type t :: %Inventory.Projection.AllStreams{streams: [{String.t, struct}]}

  defstruct streams: %{}

  defimpl Inventory.Projection do
    def project(p, %EventStore.RecordedEvent{metadata: %{"item_id" => id}} = e) do
      item = p.streams
             |> Map.get(id, %Inventory.Projection.ItemDetails{})
             |> Inventory.Projection.project(e)
      %Inventory.Projection.AllStreams{p | streams: Map.put(p.streams, id, item)}
    end
  end
end
