defmodule CommandStateStore do
  @moduledoc false
  use GenServer

  # CLIENT

  @spec start_link() :: {:ok, any}
  def start_link do
    GenServer.start_link(__MODULE__, %{}, name: __MODULE__)
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
end
