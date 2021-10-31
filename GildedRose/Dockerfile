FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["GildedRose/GildedRose.csproj", "GildedRose/"]
RUN dotnet restore "GildedRose/GildedRose.csproj"
COPY . .
WORKDIR "/src/GildedRose"
RUN dotnet build "GildedRose.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GildedRose.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GildedRose.dll"]