FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5004
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenVisStreamer.VideoLibrary/OpenVisStreamer.VideoLibrary.csproj", "OpenVisStreamer.VideoLibrary/"]
RUN dotnet restore "OpenVisStreamer.VideoLibrary/OpenVisStreamer.VideoLibrary.csproj"
COPY . .
WORKDIR "/src/OpenVisStreamer.VideoLibrary"
RUN dotnet build "OpenVisStreamer.VideoLibrary.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OpenVisStreamer.VideoLibrary.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenVisStreamer.VideoLibrary.dll"]
