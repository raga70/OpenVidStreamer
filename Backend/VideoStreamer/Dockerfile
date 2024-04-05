FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081  

RUN mkdir -p /app/data

# Install NFS client utilities
USER root
RUN apt-get update && apt-get install -y nfs-common && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VideoStreamer/VideoStreamer.csproj", "VideoStreamer/"]
RUN dotnet restore "VideoStreamer/VideoStreamer.csproj"
COPY . .
WORKDIR "/src/VideoStreamer"
RUN dotnet build "VideoStreamer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VideoStreamer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV CONSUL_IP=consul-service
ENV ASPNETCORE_URLS=http://+:8081  

WORKDIR /app
COPY --from=publish /app/publish .
COPY VideoStreamer/entrypoint.sh /entrypoint.sh
USER root
RUN apt-get update && apt-get install nfs-common -y 

RUN chmod 7777 /app/data
USER $APP_UID

ENTRYPOINT ["dotnet", "VideoStreamer.dll"]
