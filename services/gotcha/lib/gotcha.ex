defmodule Gotcha do
  def handle_message(%{key: key, value: value} = message) do
    {:ok, %{status_code: 200, body: body}} = HTTPoison.get("https://pokeapi.co/api/v2/pokemon/#{value}")
    data = Poison.decode(body)
    IO.inspect(body)
  end
end
