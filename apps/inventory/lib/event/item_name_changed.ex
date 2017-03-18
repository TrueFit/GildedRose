defmodule Inventory.Event.ItemNameChanged do
  @moduledoc false

  @type t :: %Inventory.Event.ItemNameChanged{name: String.t}

  defstruct name: ""

end
