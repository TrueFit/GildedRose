defprotocol Inventory.Projection do
  @moduledoc false

  @fallback_to_any true
  @doc "Apply an event to a projection."
  @spec project(struct, %EventStore.RecordedEvent{}) :: struct
  def project(projection, recorded_event)
end

defimpl Inventory.Projection, for: Any do
  def project(p, _), do: p
end
