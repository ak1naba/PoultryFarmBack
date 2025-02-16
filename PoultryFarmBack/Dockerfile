FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PoultryFarmBack.csproj", "./"]
RUN dotnet restore "./PoultryFarmBack.csproj"
COPY . .
RUN dotnet build "./PoultryFarmBack.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "PoultryFarmBack.dll"]
