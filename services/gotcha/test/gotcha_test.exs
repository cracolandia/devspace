defmodule GotchaTest do
  use ExUnit.Case
  doctest Gotcha

  test "greets the world" do
    assert Gotcha.hello() == :world
  end
end
