defmodule CommandProjection do
  @moduledoc false
  use GenServer

  defstruct domain_objects: %{}, next_domain_id: 0

  # CLIENT

  def start_link() do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
  end

  def next_id() do
    GenServer.call(__MODULE__, :next_id)
  end

  # SERVER
  
  def init(_) do
    {:ok, %CommandProjection{}}
  end

  def handle_call(:next_id, _from, %CommandProjection{next_domain_id: n} = state) do
    {:reply, n, %CommandProjection{state | next_domain_id: n + 1}}
  end

end
