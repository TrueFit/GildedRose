defmodule ItemValidation do
  @moduledoc false

  @categories [:weapon,
               :armor,
               :food,
               :potion,
               :sulfuras,
               :conjured,
               :backstage_passes]

  @doc """
  Test that the provided name is not blank and is a string.

  ## Example:
      iex> ItemValidation.validate_name("")
      {:error, :invalid_name}

      iex> ItemValidation.validate_name(12)
      {:error, :invalid_name}

      iex> ItemValidation.validate_name("sword")
      {:ok, "sword"}
  """
  @spec validate_name(String.t) :: {:ok, String.t} | {:error, :invalid_name}
  def validate_name(""), do: {:error, :invalid_name}
  def validate_name(name) when is_binary(name), do: {:ok, name}
  def validate_name(_), do: {:error, :invalid_name}

  @doc """
  Assert that the provided category is one of the approved
  categories.

  ## Example
      iex> ItemValidation.validate_category(:weapon)
      {:ok, :weapon}

      iex> ItemValidation.validate_category("Weapon")
      {:ok, :weapon}

      iex> ItemValidation.validate_category(:food)
      {:ok, :food}

      iex> ItemValidation.validate_category("fOOd")
      {:ok, :food}

      iex> ItemValidation.validate_category(:backstage_passes)
      {:ok, :backstage_passes}

      iex> ItemValidation.validate_category("Backstage Passes")
      {:ok, :backstage_passes}

      iex> ItemValidation.validate_category(:conjured)
      {:ok, :conjured}

      iex> ItemValidation.validate_category("Conjured")
      {:ok, :conjured}

      iex> ItemValidation.validate_category(:armor)
      {:ok, :armor}

      iex> ItemValidation.validate_category("Armor")
      {:ok, :armor}

      iex> ItemValidation.validate_category(:potion)
      {:ok, :potion}

      iex> ItemValidation.validate_category("Potion")
      {:ok, :potion}

      iex> ItemValidation.validate_category(:sulfuras)
      {:ok, :sulfuras}

      iex> ItemValidation.validate_category("Sulfuras")
      {:ok, :sulfuras}

      iex> ItemValidation.validate_category(:bad)
      {:error, :invalid_category}

      iex> ItemValidation.validate_category("bad")
      {:error, :invalid_category}
  """
  @spec validate_category(atom) :: {:ok, atom} | {:error, :invalid_category}
  def validate_category(cat) when is_atom(cat)  do
    if Enum.member?(@categories, cat) do
      {:ok, cat}
    else
      {:error, :invalid_category}
    end
  end
  def validate_category(cat) when is_binary(cat) do
    cat = cat |> String.downcase() |> String.replace(" ", "_")
    if @categories |> Enum.map(&Atom.to_string/1) |> Enum.member?(cat) do
      {:ok, String.to_atom(cat)}
    else
      {:error, :invalid_category}
    end
  end

  @doc """
  Test that the provided sell in value is a number.

  ## Example
      iex> ItemValidation.validate_sell_in(10)
      {:ok, 10}

      iex> ItemValidation.validate_sell_in(-9)
      {:ok, -9}

      iex> ItemValidation.validate_sell_in(0)
      {:ok, 0}

      iex> ItemValidation.validate_sell_in("abc")
      {:error, :invalid_sell_in}

      iex> ItemValidation.validate_sell_in(3.1415)
      {:error, :invalid_sell_in}
  """
  @spec validate_sell_in(integer) :: {:ok, integer} | {:error, :invalid_sell_in}
  def validate_sell_in(s) when is_integer(s), do: {:ok, s}
  def validate_sell_in(_), do: {:error, :invalid_sell_in}

  @doc """
  Test that the provided quality is at least zero and no more than 50 unless sulfuras.

  ## Example
      iex> ItemValidation.validate_quality(:food, 10)
      {:ok, 10}

      iex> ItemValidation.validate_quality(:sulfuras, 80)
      {:ok, 80}

      iex> ItemValidation.validate_quality(:food, -10)
      {:error, :invalid_quality}

      iex> ItemValidation.validate_quality(:food, 80)
      {:error, :invalid_quality}

      iex> ItemValidation.validate_quality(:sulfuras, 25)
      {:error, :invalid_quality}
  """
  @spec validate_quality(atom, integer) :: {:ok, integer} | {:error, :invalid_quality}
  def validate_quality(:sulfuras, 80), do: {:ok, 80}
  def validate_quality(:sulfuras, _), do: {:error, :invalid_quality}
  def validate_quality(_, quality) when quality <= 50 and quality >= 0, do: {:ok, quality}
  def validate_quality(_category, _quality), do: {:error, :invalid_quality}
end
