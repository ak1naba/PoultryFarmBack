# Базовый образ .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Сборка .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PoultryFarmBack.csproj", "./"]
RUN dotnet restore "./PoultryFarmBack.csproj"
COPY . .
WORKDIR "/src/PoultryFarmBack"
RUN dotnet build "./PoultryFarmBack.csproj" -c Release -o /app/build

# Публикация .NET
FROM build AS publish
RUN dotnet publish "./PoultryFarmBack.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ .NET
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PoultryFarmBack.dll"]
