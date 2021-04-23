#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MicroServices/Sample/DemoTools.Sample.Api/DemoTools.Sample.Api.csproj", "MicroServices/Sample/DemoTools.Sample.Api/"]
RUN dotnet restore "MicroServices/Sample/DemoTools.Sample.Api/DemoTools.Sample.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Sample/DemoTools.Sample.Api"
RUN dotnet build "DemoTools.Sample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.Sample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.Sample.Api.dll"]