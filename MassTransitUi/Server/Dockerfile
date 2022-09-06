#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MassTransitUi/Server/MassTransitUi.Server.csproj", "MassTransitUi/Server/"]
COPY ["MassTransitUi/Shared/MassTransitUi.Shared.csproj", "MassTransitUi/Shared/"]
COPY ["MassTransitUi/Client/MassTransitUi.Client.csproj", "MassTransitUi/Client/"]
RUN dotnet restore "MassTransitUi/Server/MassTransitUi.Server.csproj"
COPY . .
WORKDIR "/src/MassTransitUi/Server"
RUN dotnet build "MassTransitUi.Server.csproj" -c Release -o /app/build

FROM node:16 as nodebuild
WORKDIR /app
COPY ["MassTransitUi/Client/package.json", "package.json"]
RUN npm install
COPY ["MassTransitUi/Client/", "."]
RUN npm run tailwind-build

FROM build AS publish
RUN dotnet publish "MassTransitUi.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=nodebuild /app/publish/wwwroot/css/output.css /app/wwwroot/css/output.css
ENTRYPOINT ["dotnet", "MassTransitUi.Server.dll"]