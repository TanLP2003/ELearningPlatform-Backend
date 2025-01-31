#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y ffmpeg
USER app
WORKDIR /app
RUN mkdir storage
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/VideoManager/VideoManager.API/VideoManager.API.csproj", "Services/VideoManager/VideoManager.API/"]
RUN dotnet restore "./Services/VideoManager/VideoManager.API/VideoManager.API.csproj"
COPY . .
WORKDIR "/src/Services/VideoManager/VideoManager.API"
RUN dotnet build "./VideoManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoManager.API.dll"]