defmodule Command.AddInventoryItem do
  @moduledoc """
  Command to add a new item to inventory.
  """

  defstruct name: "", category: :unknown, sell_in: 0, quality: 0

  defimpl Command, for: Command.AddInventoryItem do
    def handle_cmd(cmd) do
      %Event.InventoryItemAdded{
        name: cmd.name,
        category: cmd.category,
        sell_in: cmd.sell_in,
        quality: cmd.quality
      }
    end
  end
end
