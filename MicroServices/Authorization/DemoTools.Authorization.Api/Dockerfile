
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicroServices/Authorization/DemoTools.Authorization.Api/*.csproj", "MicroServices/Authorization/DemoTools.Authorization.Api/"]
RUN dotnet restore "MicroServices/Authorization/DemoTools.Authorization.Api/DemoTools.Authorization.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Authorization/DemoTools.Authorization.Api"
RUN dotnet build "DemoTools.Authorization.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.Authorization.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.Authorization.Api.dll"]