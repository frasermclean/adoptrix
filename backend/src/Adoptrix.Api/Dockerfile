# This Dockerfile assumes the project has been succcessfully published to the /publish directory
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY /publish .

ENTRYPOINT [ "dotnet", "Adoptrix.Api.dll" ]
