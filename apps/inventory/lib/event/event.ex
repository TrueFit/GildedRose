defmodule Inventory.Event do
  @moduledoc false

  alias Inventory.Event.ItemAdded, as: Added
  alias Inventory.Event.InvalidItemAdded, as: InvalidAdded
  alias Inventory.Event.ItemNameChanged, as: NameChanged

  @categories [
    "weapon",
    "armor",
    "food",
    "potion",
    "sulfuras",
    "conjured",
    "backstage passes"]

  @doc """
  Create an ItemAdded event if provided with valid input.

  ## Example
      iex> Inventory.Event.item_added("", "food", 10, 10)
      {:error, :invalid_name}

      iex> Inventory.Event.item_added("banana", "bad", 10, 10)
      {:error, :invalid_category}

      iex> Inventory.Event.item_added("banana", "food", "abc", 10)
      {:error, :invalid_sell_in}

      iex> Inventory.Event.item_added("banana", "food", 10, -10)
      {:error, :invalid_quality}

      iex> Inventory.Event.item_added("banana", "food", 10, 80)
      {:error, :invalid_quality}

      iex> Inventory.Event.item_added("awesome", "sulfuras", 10, 150)
      {:error, :invalid_quality}

      iex> Inventory.Event.item_added("banana", "food", 10, 10)
      {:ok, %Inventory.Event.ItemAdded{category: "food", name: "banana", quality: 10, sell_in: 10}}

      iex> Inventory.Event.item_added("banana", "food", "10", "10")
      {:ok, %Inventory.Event.ItemAdded{category: "food", name: "banana", quality: 10, sell_in: 10}}

      iex> Inventory.Event.item_added("awesome", "sulfuras", 10, 80)
      {:ok, %Inventory.Event.ItemAdded{category: "sulfuras", name: "awesome", quality: 80, sell_in: 10}}
  """
  @spec item_added(String.t, String.t, integer, integer) :: {:ok, Added.t} | {:error, atom}
  def item_added(name, category, sell_in, quality) do
    with {:ok, n} <- item_name(name),
         {:ok, c} <- item_category(category),
         {:ok, s} <- item_sell_in(sell_in),
         {:ok, q} <- item_quality(c, quality),
     do: {:ok, %Added{name: n, category: c, sell_in: s, quality: q}}
  end

  def invalid_item_added(name, category, sell_in, quality) do
    %InvalidAdded{name: name, category: category, sell_in: sell_in, quality: quality}
  end

  @doc """
  Create an ItemNameChanged event if provided with valid input.

  ## Example
      iex> Inventory.Event.item_name_changed("")
      {:error, :invalid_name}

      iex> Inventory.Event.item_name_changed("Spear")
      {:ok, %Inventory.Event.ItemNameChanged{name: "Spear"}}
  """
  @spec item_name_changed(String.t) :: {:ok, NameChanged.t} | {:error, atom}
  def item_name_changed(new_name) do
    with {:ok, n} <- item_name(new_name),
    do: {:ok, %NameChanged{name: n}}
  end



  @spec item_name(String.t) :: {:ok, String.t} | {:error, :invalid_name}
  defp item_name(""), do: {:error, :invalid_name}
  defp item_name(name) when is_binary(name), do: {:ok, name}
  defp item_name(_), do: {:error, :invalid_name}

  @spec item_category(String.t) :: {:ok, String.t} | {:error, :invalid_category}
  defp item_category(cat) do
    c = cat |> String.downcase()
    if Enum.member?(@categories, c) do
      {:ok, c}
    else
      {:error, :invalid_category}
    end
  end

  @spec item_sell_in(integer) :: {:ok, integer} | {:error, :invalid_sell_in}
  defp item_sell_in(s) when is_integer(s), do: {:ok, s}
  defp item_sell_in(s) when is_binary(s) do
    case integer_from_string(s) do
      :error -> {:error, :invalid_sell_in}
      i -> {:ok, i}
    end
  end
  defp item_sell_in(_), do: {:error, :invalid_sell_in}

  @spec item_quality(String.t, integer) :: {:ok, integer} | {:error, :invalid_quality}
  defp item_quality(c, q) when is_binary(q) do
    case integer_from_string(q) do
      :error -> {:error, :invalid_quality}
      q_prime -> item_quality(c, q_prime)
    end
  end
  defp item_quality("sulfuras", 80), do: {:ok, 80}
  defp item_quality("sulfuras", _), do: {:error, :invalid_quality}
  defp item_quality(_, q) when q <= 50 and q >= 0, do: {:ok, q}
  defp item_quality(_, _), do: {:error, :invalid_quality}

  defp integer_from_string(s) when is_binary(s) do
    try do
      String.to_integer(s)
    rescue
      ArgumentError -> :error
    end
  end
end
