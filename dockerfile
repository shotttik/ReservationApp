FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
RUN mkdir uploads
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY wait-for-it.sh .
RUN chmod +x wait-for-it.sh
COPY --from=build /app/publish .
ENTRYPOINT ["./wait-for-it.sh", "sqlserver:1433", "--", "dotnet", "API.dll"]
