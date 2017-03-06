defmodule CommandProjection do
  @moduledoc false
  use GenServer

  @type t :: %CommandProjection{domain_objects: map, next_domain_id: integer}

  defstruct domain_objects: %{}, next_domain_id: 0

  # CLIENT

  @spec start_link() :: {:ok, any}
  def start_link do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
  end

  @spec next_id() :: integer
  def next_id do
    GenServer.call(__MODULE__, :next_id)
  end

  # SERVER

  @spec init(any) :: {:ok, CommandProjection.t}
  def init(_) do
    {:ok, %CommandProjection{}}
  end

  @spec handle_call(:next_id, any, CommandProjection.t)
    :: {:reply, integer, CommandProjection.t}
  def handle_call(:next_id, _from, %CommandProjection{next_domain_id: n} = state) do
    {:reply, n, %CommandProjection{state | next_domain_id: n + 1}}
  end

end
