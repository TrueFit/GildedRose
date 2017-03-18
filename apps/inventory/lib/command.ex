defmodule Inventory.Command do
  @moduledoc """
  """
  alias Inventory.Event, as: Event
  @state_store Application.get_env(:inventory, :command_store)

  @doc """
  Add a new item to inventory.
  """
  @spec add_item_to_inventory(String.t, atom, integer, integer) :: {:ok, String.t} | {:error, atom}
  def add_item_to_inventory(name, category, sell_in, quality) do
    with {:ok, domain_event} <- Event.item_added(name, category, sell_in, quality),
         item_id <- @state_store.next_id(),
         store_event <- create_event(domain_event, item_id, "TEST USER"),
         :ok <- EventStore.append_to_stream(item_id, 0, [store_event]),
    do: {:ok, item_id}
  end

  @doc """
  Change the new of an item in inventory.
  """
  @spec change_name(String.t, integer, String.t) :: :ok | {:error, atom}
  def change_name(item_id, version, new_name) do
    with {:ok, domain_event} <- Event.item_name_changed(new_name),
         store_event <- create_event(domain_event, item_id, "TEST USER"),
         :ok <- EventStore.append_to_stream(item_id, version, [store_event]),
    do:
      :ok
  end

  @spec create_event(struct, String.t, String.t) :: %EventStore.EventData{}
  defp create_event(inner_event, item_id, user) do
    %EventStore.EventData{
      event_type: Inventory.EventStore.JsonSerializer.to_type_string(inner_event),
      data: inner_event,
      metadata: %{user: user, item_id: item_id}
    }
  end
end
