defmodule InventoryApi.InventoryView do
  use InventoryApi.Web, :view

  def render("inventory.json", %{inventory: inventory}) do
    %{inventory: render_many(inventory, InventoryApi.InventoryView, "item.json")}
  end

  def render("item.json", %{inventory: i}) do
    %{id: i.item_id,
      version: i.version,
      name: i.name,
      category: i.category,
      sell_in: i.sell_in,
      quality: i.quality}
  end
end
