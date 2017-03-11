defmodule InventoryItemCommand do
  @moduledoc """
  """
  alias InventoryItemEventCreator, as: EventCreator
  @state_store Application.get_env(:backend, :command_store)

  @doc """
  Add a new item to inventory.
  """
  @spec add_item_to_inventory(String.t, atom, integer, integer) :: :ok | {:error, atom}
  def add_item_to_inventory(name, category, sell_in, quality) do
    with {:ok, domain_event} <- EventCreator.item_added_to_inventory(name, category, sell_in, quality),
         store_event <- create_event(domain_event, "TEST USER"),
         stream_id <- @state_store.next_id(),
         :ok <- EventStore.append_to_stream(stream_id, 0, [store_event]),
    do: :ok
  end

  @spec create_event(struct, String.t) :: %EventStore.EventData{}
  defp create_event(inner_event, user) do
    %EventStore.EventData{
      event_type: Event.type(inner_event),
      data: inner_event,
      metadata: %{user: user}
    }
  end
end
