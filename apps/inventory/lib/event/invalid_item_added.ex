defmodule Inventory.Event.InvalidItemAdded do
  @moduledoc """
  Data structure to record an item being added to inventory that is not correctly formed.
  Used when interacting with a data file or similar that cannot be corrected in real time.
  """
  alias Inventory.Event.InvalidItemAdded, as: Added

  @type t :: %Added{name: String.t, category: String.t, sell_in: integer, quality: integer}

  defstruct name: "", category: "", sell_in: 0, quality: 0
end
