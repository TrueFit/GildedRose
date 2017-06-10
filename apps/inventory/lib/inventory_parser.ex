defmodule Inventory.Parser do
  @moduledoc false

  alias Inventory.Parser.ParsingError
  alias Inventory.Parser.Item

  @doc """
  Extract item details from a comma delimited string.

  ## Examples
  
      iex> Inventory.Parser.parse_line("sword, weapon,12, 26")
      %Inventory.Parser.Item{category: "weapon", name: "sword", quality: "26", sell_in: "12"}

      iex> Inventory.Parser.parse_line("bad line")
      %Inventory.Parser.ParsingError{error: "Invalid string", line: "bad line"}
  """
  def parse_line(line) do
    with [name, category, sell_in, quality] <- String.split(line, ",") do
      Item.create(String.trim(name), String.trim(category), String.trim(sell_in), String.trim(quality))
    else
      _ -> ParsingError.create("Invalid string", line)
    end
  end

end

defmodule Inventory.Parser.Item do
  defstruct name: "", category: "", sell_in: 0, quality: 0

  def create(n, c, s, q), do: %__MODULE__{name: n, category: c, sell_in: s, quality: q}
end

defmodule Inventory.Parser.ParsingError do
  defstruct error: "Unable to parse", line: ""

  def create(e, l), do: %__MODULE__{error: e, line: l}
end
