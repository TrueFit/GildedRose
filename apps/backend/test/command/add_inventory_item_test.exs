defmodule Command.AddInventoryItemTest do
  use ExUnit.Case
  doctest Command.AddInventoryItem

  import Command, only: [handle_cmd: 1]

  test "will create an inventory item added event" do
    {:ok, event} =
      handle_cmd(mk_cmd("sword", "Weapon", 200, 30))

    assert(event.category == :weapon)
    assert(event.name == "sword")
    assert(event.quality == 30)
    assert(event.sell_in == 200)
  end

  test "will error on name that is not string" do
    {:error, :malformed} = handle_cmd(mk_cmd(18, "Food", 25, 25))
  end

  test "will error on empty name" do
    {:error, :malformed} = handle_cmd(mk_cmd("", "Food", 25, 25))
  end

  test "will error on invalid category" do
    {:error, :malformed} = handle_cmd(mk_cmd("banana", "Not a category", 25, 25))
  end

  test "will error on non-integer sell_in" do
    {:error, :malformed} = handle_cmd(mk_cmd("banana", "Food", "smith", 25))
  end

  #test "will error for quality above 50 when category is :food" do
  #  {:error, :malformed} = handle_cmd(mk_cmd("banana", :food, 25, 90))
  #end

  defp mk_cmd(n, c, s, q) do
    %Command.AddInventoryItem{name: n, category: c, sell_in: s, quality: q}
  end
end
