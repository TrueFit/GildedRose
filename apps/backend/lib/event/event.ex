defmodule Event.Event do
  @moduledoc false

  @type t :: %Event.Event{
    event_id: integer,
    domain_id: integer,
    source: String.t,
    timestamp: DateTime.t,
    payload: struct}

  defstruct event_id: -1, domain_id: -1, source: "", \
            timestamp: DateTime.utc_now, payload: nil
end
