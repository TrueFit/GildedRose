defmodule Inventory.Event.FailedAddingFromFile do
  @moduledoc """
  Event to record when a user attempted to add an item from inventory but it was not parsable.
  """

  defstruct line: ""
end
