defmodule Backend.Command do
  @moduledoc """
  Command controller that clients will interact with to change the inventory.
  """
  use GenServer

  def init() do
    GenServer.start_link(__MODULE__, %{})
  end

  @doc """
  
  """
  def add_inventory_item(_item) do
  end
  
end
