defprotocol Command do
  @moduledoc """
  Protocol for commands
  """

  def handle_cmd(cmd)
end
