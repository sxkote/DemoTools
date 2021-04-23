FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

ENV ConnectionStrings__DemoToolsRecordsDBConnection ""
ENV ASPNETCORE_ENVIRONMENT Development
ENV CoreApiURL "http://core.demo-tools.app.litskevich.ru"

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