defmodule Inventory.CommandTest do
  use ExUnit.Case

  test "happy path" do
    {:ok, id} = Inventory.Command.add_item_to_inventory("test item", "food", 23, 43)

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 1
    assert (Enum.at(result, 0)).data.name == "test item"
    assert (Enum.at(result, 0)).event_type == "ItemAdded"

    :ok = Inventory.Command.end_day()

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 2
    assert (Enum.at(result, 1)).event_type == "DayPassed"
  end

  test "valid item added through initialization process" do
    {:ok, id} = Inventory.Command.add_unsafe_item_to_inventory("test item", "food", 23, 43)

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 1
    assert (Enum.at(result, 0)).data.name == "test item"
    assert (Enum.at(result, 0)).event_type == "ItemAdded"

    :ok = Inventory.Command.end_day()

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 2
    assert (Enum.at(result, 1)).event_type == "DayPassed"
  end

  test "invalid item added through initialization process" do
    {:ok, id} = Inventory.Command.add_unsafe_item_to_inventory("test item", "", 23, 43)

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 1
    assert (Enum.at(result, 0)).data.name == "test item"
    assert (Enum.at(result, 0)).event_type == "InvalidItemAdded"

    :ok = Inventory.Command.end_day()

    result = EventStore.stream_forward(id) |> Enum.to_list()

    assert (length result) == 2
    assert (Enum.at(result, 1)).event_type == "DayPassed"
  end
end
