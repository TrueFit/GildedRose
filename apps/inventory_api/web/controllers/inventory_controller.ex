defmodule InventoryApi.InventoryController do
  use InventoryApi.Web, :controller

  def index(conn, _params) do
    render(conn, "inventory.json", inventory: Inventory.Query.inventory)
  end
end
