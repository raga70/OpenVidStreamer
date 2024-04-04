FROM ubuntu:latest AS base

# Install NFS client utilities
RUN apt-get update && \
    apt-get install -y nfs-common && \
    apt-get clean

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
RUN mkdir -p /app/data

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Upload/Upload.csproj", "Upload/"]
RUN dotnet restore "Upload/Upload.csproj"
COPY . .
WORKDIR "/src/Upload"
RUN dotnet build "Upload.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Upload.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV CONSUL_IP=consul-service

WORKDIR /app
COPY --from=publish /app/publish .
COPY Upload/entrypoint.sh /entrypoint.sh
USER root
RUN chmod +x /entrypoint.sh
USER $APP_UID
ENTRYPOINT ["/bin/sh", "/entrypoint.sh"]
