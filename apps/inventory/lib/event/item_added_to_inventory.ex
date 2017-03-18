defmodule Inventory.Event.ItemAdded do
  @moduledoc """
  Data structure to record an item being added to inventory.
  """
  alias Inventory.Event.ItemAdded, as: Added

  @type t :: %Added{name: String.t, category: String.t, sell_in: integer, quality: integer}

  defstruct name: "", category: "", sell_in: 0, quality: 0
end
