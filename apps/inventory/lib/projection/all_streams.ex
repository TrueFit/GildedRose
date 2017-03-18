defmodule Inventory.Projection.AllStreams do
  @moduledoc false

  @type t :: %Inventory.Projection.AllStreams{streams: [{String.t, integer}]}

  defstruct streams: %{}

  defimpl Inventory.Projection do
    def project(p, %EventStore.RecordedEvent{stream_version: v, metadata: %{"item_id" => id}}) do
      %Inventory.Projection.AllStreams{p | streams: Map.put(p.streams, id, v)}
    end
  end
end
