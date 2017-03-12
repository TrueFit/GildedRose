defmodule Command.StateStore do
  @moduledoc false
  use GenServer

  @subscription "command_subscription"

  # CLIENT

  @spec start_link() :: {:ok, any}
  def start_link do
    with {:ok, pid} <- GenServer.start_link(__MODULE__, %{}, name: __MODULE__),
         {:ok, _subscription} <- EventStore.subscribe_to_all_streams(@subscription, pid),
    do: {:ok, pid}
  end

  def stream_exists?(item_id) do
    GenServer.call(__MODULE__, {:stream_exists, item_id})
  end

  def inventory_item_state(item_id) do
    GenServer.call(__MODULE__, {:get_item, item_id})
  end

  @spec next_id() :: String.t
  def next_id do
    UUID.uuid4()
  end

  # SERVER

  @spec init(any) :: {:ok, map}
  def init(_) do
    {:ok, %{}}
  end

  def handle_call({:stream_exists, stream_id}, _from, state) do
    {:reply, Map.has_key?(state, stream_id), state}
  end

  def handle_call({:get_item, item_id}, _from, state) do
    {:reply, Map.get(state, item_id), state}
  end

  def handle_info({:events, events, subscription}, state) do
    send(subscription, {:ack, List.last(events).event_id})

    IO.inspect(List.first(events))

    {:noreply, state}
  end
end
