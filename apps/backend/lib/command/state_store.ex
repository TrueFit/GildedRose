defmodule Command.StateStore do
  @moduledoc false
  use GenServer
  require Logger

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

  def get_item_state(item_id) do
    GenServer.call(__MODULE__, {:get_item, item_id})
  end

  # def get_item_ids() do
  #   GenServer.call(__MODULE__, :get_item_ids)
  # end

  @spec next_id() :: String.t
  def next_id do
    UUID.uuid4()
  end

  # SERVER

  @spec init(any) :: {:ok, map}
  def init(_) do
    Logger.debug("Initializing Command.StateStore")
    {:ok, events} = EventStore.read_all_streams_forward()

    state = events
    |> Enum.group_by(fn x -> Map.get(x.metadata, "item_id") end)
    |> Map.values()
    |> Enum.map(fn x -> process_events(x, %{}) end)
    |> Enum.reduce(&Map.merge/2)

    {:ok, state}
  end

  def handle_call({:stream_exists, stream_id}, _from, state) do
    {:reply, Map.has_key?(state, stream_id), state}
  end

  def handle_call({:get_item, _item_id}, _from, state) do
    {:reply, state, state}
  end

  # def handle_call(:get_item_ids, _from, state) do
  #   {:reply, Map.keys(state), state}
  # end

  def handle_info({:events, events, subscription}, state) do
    last_event = List.last(events)
    Logger.debug("Command.StateStore: Recieved events")

    send(subscription, {:ack, last_event.event_id})

    state = process_events(events, state)

    {:noreply, state}
  end

  defp process_events(events, state) do
    item_id = Map.get(List.first(events).metadata, "item_id")

    initial_state = Map.get(state, item_id)
    item = events |> Enum.reduce(initial_state, &Command.EventAggregate.aggregate/2)

    Map.put(state, item_id, item)
  end
end

defmodule Command.Item do
  @moduledoc false

  @type type_t :: %Command.Item{name: String.t, category: String.t, sell_in: integer, quality: integer, item_id: String.t}

  defstruct name: "", category: "", sell_in: 0, quality: 0, item_id: ""
end

defmodule Command.EventAggregate do
  @moduledoc false


  def aggregate(%EventStore.RecordedEvent{data: event, metadata: %{"item_id" => id}}, nil), do: agg(event, %Command.Item{item_id: id})
  def aggregate(%EventStore.RecordedEvent{data: event}, state), do: agg(event, state)

  defp agg(%Event.ItemAddedToInventory{name: n, category: c, sell_in: s, quality: q}, state) do
    %Command.Item{state | name: n, category: c, sell_in: s, quality: q}
  end
end
