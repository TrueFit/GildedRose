defmodule Inventory.Parser do
  @moduledoc false

  alias Inventory.Parser.Error
  alias Inventory.Parser.Item

  @doc """
  Extract item details from a comma delimited string.

  ## Examples
  
      iex> Inventory.Parser.parse_line("sword, weapon,12, 26")
      %Inventory.Parser.Item{category: "weapon", name: "sword", quality: "26", sell_in: "12"}

      iex> Inventory.Parser.parse_line("bad line")
      %Inventory.Parser.Error{error: "Invalid string", line: "bad line"}
  """
  def parse_line(line) do
    with [name, category, sell_in, quality] <- String.split(line, ",") do
      Item.create(String.trim(name), String.trim(category), String.trim(sell_in), String.trim(quality))
    else
      _ -> Error.create("Invalid string", line)
    end
  end

  @doc """
  Transform a stream of strings into Parser structs (Item or Error)
  """
  def parse(stream) do
    stream
    |> Stream.map(&parse_line/1)
  end

end

defmodule Inventory.Parser.Item do
  defstruct name: "", category: "", sell_in: 0, quality: 0

  def create(n, c, s, q), do: %__MODULE__{name: n, category: c, sell_in: s, quality: q}
end

defmodule Inventory.Parser.Error do
  defstruct error: "Unable to parse", line: ""

  def create(e, l), do: %__MODULE__{error: e, line: l}
end
