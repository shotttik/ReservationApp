FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN mkdir -p /app/uploads
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ReservationApp.sln", "./"]
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Application.Tests/Application.Tests.csproj", "Application.Tests/"]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "ReservationApp.sln"

COPY . .
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish "API/API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY wait-for-it.sh .
RUN chmod +x wait-for-it.sh
COPY --from=build /app/publish .
ENTRYPOINT ["sh", "-c", "./wait-for-it.sh sqlserver:1433 --timeout=30 -- dotnet API.dll || echo 'Startup failed, exiting...'"]
