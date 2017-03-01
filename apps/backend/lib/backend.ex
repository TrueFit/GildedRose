defmodule Backend do
  @moduledoc false

  use Application

  def start(_type, _args) do
    Backend.Supervisor.start_link
  end
end

defmodule Backend.Supervisor do
  @moduledoc false
  use Supervisor

  def start_link do
    Supervisor.start_link(__MODULE__, :ok)
  end

  def init(:ok) do
      children = [
        worker(CommandProjection, [])
      ]

      supervise(children, strategy: :one_for_one)
  end
end
