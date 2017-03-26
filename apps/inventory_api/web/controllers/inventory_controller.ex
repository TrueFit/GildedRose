defmodule InventoryApi.InventoryController do
  use InventoryApi.Web, :controller

  def index(conn, _params) do
    render(conn, "inventory.json", inventory: Inventory.Query.inventory)
  end

  def create(conn, params) do
    n = Map.get(params, "name")
    c = Map.get(params, "category")
    s = Map.get(params, "sell_in")
    q = Map.get(params, "quality")

    case Inventory.Command.add_item_to_inventory(n, c, s, q) do
      {:ok, id} -> conn |> render("item_id.json", item_id: id)
      {:error, error} -> conn |> put_status(400) |> render("error.json", message: error)
    end

  end
end
