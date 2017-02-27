defmodule Command.AddInventoryItem do
  @moduledoc """
  Command to add a new item to inventory.
  """

  defstruct name: "", category: "", sell_in: 0, quality: 0

end

defimpl Command, for: Command.AddInventoryItem do
  def handle_cmd(cmd) do
    with {:ok, name} <- validate_name(cmd.name),
      {:ok, category} <- validate_category(cmd.category),
      {:ok, sell_in} <- validate_sell_in(cmd.sell_in),
      {:ok, quality} <- validate_quality(cmd.name, cmd.category, cmd.quality),
    do: {:ok, %Event.InventoryItemAdded{name: name, category: category, sell_in: sell_in, quality: quality}}
  end

  defp validate_name(""), do: {:error, :malformed}
  defp validate_name(name) when is_binary(name), do: {:ok, name}
  defp validate_name(_), do: {:error, :malformed}

  defp validate_category("Weapon"), do: {:ok, :weapon}
  defp validate_category("Food"), do: {:ok, :food}
  defp validate_category("Backstage Passes"), do: {:ok, :backstage_passes}
  defp validate_category("Conjured"), do: {:ok, :conjured}
  defp validate_category("Armor"), do: {:ok, :armor}
  defp validate_category("Potion"), do: {:ok, :potion}
  defp validate_category("Sulfuras"), do: {:ok, :sulfuras}
  defp validate_category(_), do: {:error, :malformed}

  defp validate_sell_in(s) when is_integer(s), do: {:ok, s}
  defp validate_sell_in(_), do: {:error, :malformed}

  defp validate_quality(_name, _category, quality), do: {:ok, quality}
end
