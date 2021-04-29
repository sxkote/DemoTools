
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicroServices/Notifications/DemoTools.Notifications.Api/DemoTools.Notifications.Api.csproj", "MicroServices/Notifications/DemoTools.Notifications.Api/"]
RUN dotnet restore "MicroServices/Notifications/DemoTools.Notifications.Api/DemoTools.Notifications.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Notifications/DemoTools.Notifications.Api"
RUN dotnet build "DemoTools.Notifications.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.Notifications.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.Notifications.Api.dll"]
