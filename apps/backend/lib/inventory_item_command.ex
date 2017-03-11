defmodule InventoryItemCommand do
  @moduledoc """
  """
  alias InventoryItemEventCreator, as: EventCreator

  @doc """
  Validate command and create an ItemAddedToInventory event.
  """
  @spec add_item_to_inventory(String.t, atom, integer, integer) :: :ok | {:error, atom}
  def add_item_to_inventory(name, category, sell_in, quality) do
    with {:ok, domain_event} <- EventCreator.item_added_to_inventory(name, category, sell_in, quality),
         store_event <- create_event(domain_event, "TEST USER"),
         :ok <- EventStore.append_to_stream("1", 0, [store_event]),
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
