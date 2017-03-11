defprotocol Event do
  @moduledoc false

  @spec type(struct) :: String.t
  def type(event)
end

defimpl Event, for: Event.ItemAddedToInventory do
    def type(_), do: "ItemAddedToInventory"
end
