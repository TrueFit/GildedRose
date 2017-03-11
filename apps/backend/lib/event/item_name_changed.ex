defmodule Event.ItemNameChanged do
  @moduledoc false

  @type t :: %Event.ItemNameChanged{name: String.t}

  defstruct name: ""

end
