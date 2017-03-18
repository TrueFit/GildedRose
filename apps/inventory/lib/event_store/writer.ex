defmodule Inventory.EventStore.Writer do
  @moduledoc false

  @doc """
  """
  @spec write(String.t, String.t, integer, [struct]) :: :ok | {:error, atom}
  def write(item_id, user, version, domain_events) do
    domain_events
    |> Enum.map(fn e -> event(item_id, user, e) end)
    |> persist(item_id, version)
  end

  defp event(item_id, user, domain_event) do
    %EventStore.EventData{
      event_type: Inventory.EventStore.JsonSerializer.to_type_string(domain_event),
      metadata: %{
        user: user,
        item_id: item_id
      },
      data: domain_event
    }
  end

  defp persist(events, item_id, version), do: EventStore.append_to_stream(item_id, version, events)
end
