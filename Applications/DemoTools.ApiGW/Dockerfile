#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Applications/DemoTools.ApiGW/DemoTools.ApiGW.csproj", "Applications/DemoTools.ApiGW/"]
RUN dotnet restore "Applications/DemoTools.ApiGW/DemoTools.ApiGW.csproj"
COPY . .
WORKDIR "/src/Applications/DemoTools.ApiGW"
RUN dotnet build "DemoTools.ApiGW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.ApiGW.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.ApiGW.dll"]