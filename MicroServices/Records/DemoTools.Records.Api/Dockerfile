#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicroServices/Records/DemoTools.Records.Api/DemoTools.Records.Api.csproj", "MicroServices/Records/DemoTools.Records.Api/"]
RUN dotnet restore "MicroServices/Records/DemoTools.Records.Api/DemoTools.Records.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Records/DemoTools.Records.Api"
RUN dotnet build "DemoTools.Records.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.Records.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.Records.Api.dll"]