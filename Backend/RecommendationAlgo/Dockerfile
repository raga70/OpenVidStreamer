FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081  


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecommendationAlgo/RecommendationAlgo.csproj", "RecommendationAlgo/"]
RUN dotnet restore "RecommendationAlgo/RecommendationAlgo.csproj"
COPY . .
WORKDIR "/src/RecommendationAlgo"
RUN dotnet build "RecommendationAlgo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RecommendationAlgo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV CONSUL_IP=consul-service
ENV ASPNETCORE_URLS=http://+:8081  

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecommendationAlgo.dll"]
