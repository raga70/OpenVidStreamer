﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecomendationAlgo/RecomendationAlgo.csproj", "RecomendationAlgo/"]
RUN dotnet restore "RecomendationAlgo/RecomendationAlgo.csproj"
COPY . .
WORKDIR "/src/RecomendationAlgo"
RUN dotnet build "RecomendationAlgo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RecomendationAlgo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV CONSUL_IP=consul-service
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecomendationAlgo.dll"]
