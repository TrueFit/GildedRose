defmodule Event.ItemAddedToInventory do
  @moduledoc """
  Data structure to record an item being added to inventory.
  """
  @type t :: %Event.ItemAddedToInventory{name: String.t, category: atom, sell_in: integer, quality: integer}

  defstruct name: "", category: :unknown, sell_in: 0, quality: 0
end
