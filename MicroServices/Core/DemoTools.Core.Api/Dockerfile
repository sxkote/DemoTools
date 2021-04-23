FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

ENV ConnectionStrings__DemoToolsCoreDBConnection ""
ENV ASPNETCORE_ENVIRONMENT Development
ENV SMTPServerConfig ""

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicroServices/Core/DemoTools.Core.Api/*.csproj", "MicroServices/Core/DemoTools.Core.Api/"]
RUN dotnet restore "MicroServices/Core/DemoTools.Core.Api/DemoTools.Core.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Core/DemoTools.Core.Api"
RUN dotnet build "DemoTools.Core.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoTools.Core.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoTools.Core.Api.dll"]