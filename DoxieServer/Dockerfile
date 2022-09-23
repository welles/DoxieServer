FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS restore
WORKDIR /src
COPY . .
WORKDIR /src/DoxieServer
RUN dotnet restore "DoxieServer.csproj"

FROM restore AS build
RUN dotnet build "DoxieServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoxieServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoxieServer.dll"]
