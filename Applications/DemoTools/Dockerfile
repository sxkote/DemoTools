#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

# install NodeJS
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq

# install Vue-Cli
RUN npm install -g @vue/cli 
RUN npm install -g @vue/cli-service 

# copy package.json
COPY ["DemoTools/ClientApp/package*.json", "/src/DemoTools/ClientApp/"]

# install NPM packages
WORKDIR "/src/DemoTools/ClientApp"
RUN npm install 

WORKDIR /app

EXPOSE 80
ENV ConnectionStrings__DemoDBConnection "Host=192.168.1.173;Port=32770;Database=DemoDB;Username=demo-user;Password=123456"
ENV ASPNETCORE_ENVIRONMENT Development
ENV SMTPServerConfig "Server=smtp.yandex.ru;Login=;Password=;Port=587;SSL=true;From="

# restore NuGet packages
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DemoTools/DemoTools.csproj", "DemoTools/"]
RUN dotnet restore "DemoTools/DemoTools.csproj"

# build application
COPY . .
WORKDIR "/src/DemoTools"
RUN dotnet build "DemoTools.csproj" -c Release -o /app/build

# publish application
FROM build AS publish
RUN dotnet publish "DemoTools.csproj" -c Release -o /app/publish

# copy publish to /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=base /src/DemoTools/ClientApp/node_modules ./ClientApp/node_modules

COPY "DemoTools/ClientApp/src" ./ClientApp/src
COPY "DemoTools/ClientApp/public" ./ClientApp/public


ENTRYPOINT ["dotnet", "DemoTools.dll"]