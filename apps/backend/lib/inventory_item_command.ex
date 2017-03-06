defmodule InventoryItemCommand do
  @moduledoc """
  """
  @state_store Application.get_env(:backend, :write_projector)

  @doc """
  Validate command and create an ItemAddedToInventory event.
  """
  @spec add_item_to_inventory(String.t, atom, integer, integer)
    :: {:ok, Event.Event.t} | {:error, :malformed}
  def add_item_to_inventory(name, category, sell_in, quality) do
    with {:ok, n} <- ItemValidation.validate_name(name),
      {:ok, c} <- ItemValidation.validate_category(category),
      {:ok, s} <- ItemValidation.validate_sell_in(sell_in),
      {:ok, q} <- ItemValidation.validate_quality(c, quality),
      payload <- added_event(n, c, s, q),
      domain_id <- @state_store.next_id,
    do: {:ok, %Event.Event{
      event_id: 0,
      domain_id: domain_id,
      source: "testing",
      payload: payload}}
  end

  defp added_event(n, c, s, q) do
    %Event.ItemAddedToInventory{name: n, category: c, sell_in: s, quality: q}
  end

end
