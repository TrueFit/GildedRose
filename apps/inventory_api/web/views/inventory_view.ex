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

  def render("error.json", %{message: msg}) do
    %{error: msg}
  end

  def render("item_id.json", %{item_id: id}) do
    %{item_id: id}
  end

  def render("success.json", _) do
    %{success: true}
  end
end
