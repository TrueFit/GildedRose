defmodule Inventory.Domain.Item do
  @moduledoc false

  @doc """
  Create new inventory item values from previous values.

  At the end of each day, the sell_in decreases by one. The quality
  changes based on the name, category, and age of the item.
  
  * Quality is never less than 0 or more than 50
  * Normal items decrease in quality by one
  * Items with a sell_in of less than zero there quality twice as fast
  * Aged Brie increases in quality by one
  * Conjured items degrade in quality twice as fast
  * Sulfuras items always have a quality of 80
  * Backstage passes increase in value 
  
  ## Examples
      iex> Inventory.Domain.Item.age({"pear", "food", 10, 5})
      {"pear", "food", 9, 4}

      iex> Inventory.Domain.Item.age({"pear", "food", 0, 5})
      {"pear", "food", -1, 3}

      iex> Inventory.Domain.Item.age({"pear", "food", 1, 0})
      {"pear", "food", 0, 0}

      iex> Inventory.Domain.Item.age({"pear", "food", 0, 0})
      {"pear", "food", -1, 0}

      iex> Inventory.Domain.Item.age({"pear", "food", 0, 1})
      {"pear", "food", -1, 0}

      iex> Inventory.Domain.Item.age({"aged brie", "food", 4, 10})
      {"aged brie", "food", 3, 11}

      iex> Inventory.Domain.Item.age({"aged brie", "food", 0, 10})
      {"aged brie", "food", -1, 11}

      iex> Inventory.Domain.Item.age({"aged brie", "food", -1, 10})
      {"aged brie", "food", -2, 11}

      iex> Inventory.Domain.Item.age({"amulet", "conjured", 4, 10})
      {"amulet", "conjured", 3, 8}

      iex> Inventory.Domain.Item.age({"amulet", "conjured", 4, 1})
      {"amulet", "conjured", 3, 0}

      iex> Inventory.Domain.Item.age({"amulet", "conjured", 0, 10})
      {"amulet", "conjured", -1, 6}

      iex> Inventory.Domain.Item.age({"amulet", "conjured", 0, 3})
      {"amulet", "conjured", -1, 0}

      iex> Inventory.Domain.Item.age({"awesome", "sulfuras", 4, 80})
      {"awesome", "sulfuras", 4, 80}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 11, 10})
      {"kermit", "backstage passes", 10, 11}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 10, 11})
      {"kermit", "backstage passes", 9, 13}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 5, 13})
      {"kermit", "backstage passes", 4, 16}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 0, 16})
      {"kermit", "backstage passes", -1, 0}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 4, 49})
      {"kermit", "backstage passes", 3, 50}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 9, 49})
      {"kermit", "backstage passes", 8, 50}

      iex> Inventory.Domain.Item.age({"kermit", "backstage passes", 19, 50})
      {"kermit", "backstage passes", 18, 50}
  """
  def age({n, "sulfuras", s, _}), do: {n, "sulfuras", s, 80}
  def age({n, c, s, _} = t), do: {n, c, s - 1, quality(t)}

  def change_name({_, c, s, q}, new_name), do: {new_name, c, s, q}
  def change_category({n, _, s, q}, new_cat), do: {n, new_cat, s, q}
  def change_sell_in({n, c, _, q}, new_sell_in), do: {n, c, new_sell_in, q}
  def change_quality({n, c, s, _}, new_quality), do: {n, c, s, new_quality}

  defp quality(t), do: t |> calc_quality() |> bracket()

  defp calc_quality({_, "backstage passes", s, q}) when s > 10, do: q + 1
  defp calc_quality({_, "backstage passes", s, q}) when s > 5, do: q + 2
  defp calc_quality({_, "backstage passes", s, q}) when s > 0, do: q + 3
  defp calc_quality({_, "backstage passes", _, _}), do: 0
  defp calc_quality({_, "conjured", s, q}) when s <= 0, do: q - 4
  defp calc_quality({_, "conjured", _, q}), do: q - 2
  defp calc_quality({"aged brie", "food", _, q}), do: q + 1
  defp calc_quality({_, _, s, q}) when s <= 0, do: q - 2
  defp calc_quality({_, _, _, q}), do: q - 1

  defp bracket(q) when q > 50, do: 50
  defp bracket(q) when q < 0, do: 0
  defp bracket(q), do: q
end
