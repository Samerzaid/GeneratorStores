# base local debug
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY GeneratorStores.Presentation.UI/GeneratorStores.Presentation.UI.csproj GeneratorStores.Presentation.UI/
COPY GeneratorStores.API/GeneratorStores.API.csproj GeneratorStores.API/
COPY GeneratorStores.DataAccess/GeneratorStores.DataAccess.csproj GeneratorStores.DataAccess/


RUN dotnet restore GeneratorStores.API/GeneratorStores.API.csproj

COPY . .
WORKDIR /src/GeneratorStores.API
RUN dotnet build GeneratorStores.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish GeneratorStores.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeneratorStores.API.dll"]
