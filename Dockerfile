# base local debug
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Restore stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files
COPY ./GeneratorStores.Presentation.UI/GeneratorStores.Presentation.UI.csproj ./GeneratorStores.Presentation.UI/
COPY ./GeneratorStores.API/GeneratorStores.API.csproj ./GeneratorStores.API/
COPY ./GeneratorStores.DataAccess/GeneratorStores.DataAccess.csproj ./GeneratorStores.DataAccess/


# Restore
RUN dotnet restore ./GeneratorStores.API/GeneratorStores.API.csproj

# Copy everything else
COPY . .

# Build
WORKDIR /src/GeneratorStores.API
RUN dotnet build GeneratorStores.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish GeneratorStores.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeneratorStores.API.dll"]
