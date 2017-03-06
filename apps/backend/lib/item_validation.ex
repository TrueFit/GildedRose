defmodule ItemValidation do
  @moduledoc """
  Collection of functions to validate random input matches requirements for an inventory item.
  """
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
      {:error, :malformed}

      iex> ItemValidation.validate_name(12)
      {:error, :malformed}

      iex> ItemValidation.validate_name("sword")
      {:ok, "sword"}
  """
  @spec validate_name(String.t) :: {:ok, String.t} | {:error, :malformed}
  def validate_name(""), do: {:error, :malformed}
  def validate_name(name) when is_binary(name), do: {:ok, name}
  def validate_name(_), do: {:error, :malformed}

  @doc """
  Assert that the provided category is one of the approved
  categories.

  ## Example
      iex> ItemValidation.validate_category(:weapon)
      {:ok, :weapon}

      iex> ItemValidation.validate_category(:food)
      {:ok, :food}

      iex> ItemValidation.validate_category(:backstage_passes)
      {:ok, :backstage_passes}

      iex> ItemValidation.validate_category(:conjured)
      {:ok, :conjured}

      iex> ItemValidation.validate_category(:armor)
      {:ok, :armor}

      iex> ItemValidation.validate_category(:potion)
      {:ok, :potion}

      iex> ItemValidation.validate_category(:sulfuras)
      {:ok, :sulfuras}

      iex> ItemValidation.validate_category(:bad)
      {:error, :malformed}
  """
  @spec validate_category(atom) :: {:ok, atom} | {:error, :malformed}
  def validate_category(cat) do
    if Enum.member?(@categories, cat) do
      {:ok, cat}
    else
      {:error, :malformed}
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
      {:error, :malformed}

      iex> ItemValidation.validate_sell_in(3.1415)
      {:error, :malformed}
  """
  @spec validate_sell_in(integer) :: {:ok, integer} | {:error, :malformed}
  def validate_sell_in(s) when is_integer(s), do: {:ok, s}
  def validate_sell_in(_), do: {:error, :malformed}

  @doc """
  Test that the provided quality is at least zero and no more than 50 unless sulfuras.

  ## Example
      iex> ItemValidation.validate_quality(:food, 10)
      {:ok, 10}

      iex> ItemValidation.validate_quality(:sulfuras, 80)
      {:ok, 80}

      iex> ItemValidation.validate_quality(:food, -10)
      {:error, :malformed}

      iex> ItemValidation.validate_quality(:food, 80)
      {:error, :malformed}

      iex> ItemValidation.validate_quality(:sulfuras, 25)
      {:error, :malformed}
  """
  @spec validate_quality(atom, integer) :: {:ok, integer} | {:error, :malformed}
  def validate_quality(:sulfuras, 80), do: {:ok, 80}
  def validate_quality(:sulfuras, _), do: {:error, :malformed}
  def validate_quality(_, quality) when quality <= 50 and quality >= 0, do: {:ok, quality}
  def validate_quality(_category, _quality), do: {:error, :malformed}
end
