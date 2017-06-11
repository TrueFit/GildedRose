defmodule Inventory.Command do
  @moduledoc """
  """
  alias Inventory.Event, as: Event

  def initialize_inventory_from_file(filepath) do
    if File.exists?(filepath) and !Inventory.EventStore.Reader.initialized?() do
      filepath
      |> File.stream!()
      |> Inventory.Parser.parse()
      |> Enum.map(&add_unsafe_item_to_inventory/1)
    else
      {:ok, "already initialized"}
    end
  end

  @doc """
  Add a new item to inventory.
  """
  @spec add_item_to_inventory(String.t, atom, integer, integer) :: {:ok, String.t} | {:error, atom}
  def add_item_to_inventory(name, category, sell_in, quality) do
    with {:ok, domain_event} <- Event.item_added(name, category, sell_in, quality),
         item_id <- UUID.uuid4(),
         :ok <- Inventory.EventStore.Writer.write(item_id, "TEST USER", 0, [domain_event]),
    do: {:ok, item_id}
  end

  def add_unsafe_item_to_inventory(name, category, sell_in, quality) do
    event = case Event.item_added(name, category, sell_in, quality) do
      {:ok, domain_event} -> domain_event
      {:error, _} -> Event.invalid_item_added(name, category, sell_in, quality)
    end

    item_id = UUID.uuid4()

    with :ok <- Inventory.EventStore.Writer.write(item_id, "TEST USER", 0, [event]), do: {:ok, item_id}
  end

  def add_unsafe_item_to_inventory(%Inventory.Parser.Item{name: n, category: c, sell_in: s, quality: q}) do
    add_unsafe_item_to_inventory(n, c, s, q)
  end

  def add_unsafe_item_to_inventory(%Inventory.Parser.Error{line: l}) do
    item_id = UUID.uuid4()
    event = %Event.FailedAddingFromFile{line: l}

    Inventory.EventStore.Writer.write(item_id, "TEST USER", 0, [event])

    {:ok, item_id}
  end

  @doc """
  Change the new of an item in inventory.
  """
  @spec change_name(String.t, integer, String.t) :: :ok | {:error, atom}
  def change_name(item_id, version, new_name) do
    with {:ok, domain_event} <- Event.item_name_changed(new_name),
    do: Inventory.EventStore.Writer.write(item_id, "TEST USER", version, [domain_event])
  end

  @spec end_day :: :ok | {:error, atom}
  def end_day do
    Inventory.EventStore.Writer.write_to_all_streams("TEST USER", [%Inventory.Event.DayPassed{}])
  end

end
