FROM elixir:latest
MAINTAINER Jason Hursey <jh224606@gmail.com>

LABEL version="2017-03-23"

# Install postgres client
RUN apt-get update && apt-get install -y postgresql-client

# Install hex, rebar, phoenix, and openmaize
RUN mix local.hex --force \
    && mix local.rebar --force \
    && mix archive.install --force https://github.com/phoenixframework/archives/raw/master/phoenix_new.ez \
    && mix archive.install --force https://github.com/riverrun/openmaize/raw/master/installer/archives/openmaize_phx.ez

# Application directory
RUN mkdir /app
WORKDIR /app
ADD . /app

# Phoenix port
EXPOSE 4000

# RUN event_store.create
RUN mix deps.get 

CMD mix event_store.create && mix phoenix.server
