FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["TimescaleAPI/TimescaleAPI.csproj", "TimescaleAPI/"]
RUN dotnet restore "TimescaleAPI/TimescaleAPI.csproj"

COPY . .

WORKDIR /src/TimescaleAPI

RUN dotnet publish -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TimescaleAPI.dll"]