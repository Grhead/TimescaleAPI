FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

COPY ["TimescaleAPI/TimescaleAPI.csproj", "TimescaleAPI/"]
COPY ["TimescaleAPI.Tests/TimescaleAPI.Tests.csproj", "TimescaleAPI.Tests/"]

RUN dotnet restore "TimescaleAPI.Tests/TimescaleAPI.Tests.csproj"

COPY . .

WORKDIR /src/TimescaleAPI.Tests

CMD ["dotnet", "test", "--configuration", "Release", "--no-restore"]