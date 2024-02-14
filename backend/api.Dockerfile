FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and publish the project
COPY . ./
RUN dotnet restore ./src/Adoptrix.Api/Adoptrix.Api.csproj
RUN dotnet publish ./src/Adoptrix.Api/Adoptrix.Api.csproj --configuration Release --output publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "Adoptrix.Api.dll" ]
