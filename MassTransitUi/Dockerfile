#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MassTransitUi/MassTransitUi.csproj", "MassTransitUi/"]
RUN dotnet restore "MassTransitUi/MassTransitUi.csproj"
COPY . .
WORKDIR "/src/MassTransitUi"
RUN dotnet build "MassTransitUi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransitUi.csproj" -c Release -o /app/publish

FROM node:12 as nodebuild
WORKDIR /app
COPY MassTransitUi/clientapp/package.json .
RUN npm install
COPY MassTransitUi/clientapp/ .
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=nodebuild /app/dist ./clientdist
ENTRYPOINT ["dotnet", "MassTransitUi.dll"]