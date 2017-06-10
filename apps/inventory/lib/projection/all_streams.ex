defmodule Inventory.Projection.AllStreams do
  @moduledoc false

  @type t :: %Inventory.Projection.AllStreams{streams: [{String.t, struct}]}
  alias Inventory.Projection.AllStreams, as: AllStreams

  defstruct streams: %{}

  @doc """
  """
  def projection(stream) do
    projection(stream, %AllStreams{})
  end

  def projection(stream, initial) do
    stream
    |> Enum.reduce(initial, &(project(&2, &1)))
    |> (fn x -> x.streams end).()
    |> Map.values()
  end


  defp project(p, %EventStore.RecordedEvent{metadata: %{"item_id" => id}} = e) do
    item = p.streams
           |> Map.get(id, %Inventory.Projection.ItemDetails{})
           |> Inventory.Projection.project(e)
    %Inventory.Projection.AllStreams{p | streams: Map.put(p.streams, id, item)}
  end
end

