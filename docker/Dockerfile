FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Yasy.slnx .
COPY src/Yasy.Core/Yasy.Core.csproj src/Yasy.Core/
COPY src/Yasy.Shared/Yasy.Shared.csproj src/Yasy.Shared/
COPY src/Yasy.Infrastructure/Yasy.Infrastructure.csproj src/Yasy.Infrastructure/
COPY src/Yasy.Server/Yasy.Server.csproj src/Yasy.Server/
RUN dotnet restore Yasy.slnx

COPY src/ src/
RUN dotnet publish src/Yasy.Server/Yasy.Server.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Yasy.Server.dll"]
