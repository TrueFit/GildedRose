defmodule Inventory.EventStore.Writer do
  @moduledoc false

  @doc """
  """
  @spec write(String.t, String.t, integer, [struct]) :: :ok | {:error, atom}
  def write(item_id, user, version, domain_events) do
    domain_events
    |> Enum.map(fn e -> create_event(item_id, user, e) end)
    |> persist(item_id, version)
  end

  @spec write_to_all_streams(String.t, [struct]) :: :ok | {:error, atom}
  def write_to_all_streams(user, domain_events) do
    Inventory.Query.inventory()
      |> Enum.map(fn state -> {state, create_event(state.item_id, user, domain_events)} end)
      |> Enum.map(fn {state, es} -> persist(es, state.item_id, state.version) end)
  end

  defp create_event(item_id, user, domain_events) when is_list(domain_events) do
    domain_events |> Enum.map(fn e -> create_event(item_id, user, e) end)
  end
  defp create_event(item_id, user, domain_event) do
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
