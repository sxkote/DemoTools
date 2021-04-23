#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Applications/DemoTools.ApiGateWay/DemoTools.ApiGateWay.csproj", "Applications/DemoTools.ApiGateWay/"]
RUN dotnet restore "Applications/DemoTools.ApiGateWay/DemoTools.ApiGateWay.csproj"
COPY . .
WORKDIR "/src/Applications/DemoTools.ApiGateWay"
RUN dotnet build "DemoTools.ApiGateWay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.ApiGateWay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.ApiGateWay.dll"]