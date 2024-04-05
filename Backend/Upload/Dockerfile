FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081  

RUN mkdir -p /app/data

USER root
RUN apt-get update && apt-get install -y nfs-common && rm -rf /var/lib/apt/lists/*


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
ENV ASPNETCORE_URLS=http://+:8081  

WORKDIR /app
COPY --from=publish /app/publish .
COPY Upload/entrypoint.sh /entrypoint.sh
USER root
RUN apt-get update && apt-get install nfs-common -y 
RUN chmod 7777 /app/data
USER $APP_UID
ENTRYPOINT ["dotnet", "Upload.dll"]


