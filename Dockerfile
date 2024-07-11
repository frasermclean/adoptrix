# Build project from SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore ./src/Adoptrix/Adoptrix.csproj
RUN dotnet publish ./src/Adoptrix/Adoptrix.csproj --configuration Release --output publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "Adoptrix.dll" ]
