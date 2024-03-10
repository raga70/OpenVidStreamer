FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Install FFmpeg
RUN apt-get update && \
    apt-get install -y ffmpeg && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
RUN mkdir -p /app/data

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Render/Render.csproj", "Render/"]
RUN dotnet restore "Render/Render.csproj"
COPY . .
WORKDIR "/src/Render"
RUN dotnet build "Render.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Render.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV CONSUL_IP=consul-service

WORKDIR /app
COPY --from=publish /app/publish .
COPY Render/entrypoint.sh /entrypoint.sh
USER root
RUN chmod +x /entrypoint.sh
USER $APP_UID


ENTRYPOINT ["/bin/sh", "/entrypoint.sh"]