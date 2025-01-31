#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
RUN mkdir storage
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/VideoLibrary/VideoLibrary.API/VideoLibrary.API.csproj", "Services/VideoLibrary/VideoLibrary.API/"]
RUN dotnet restore "./Services/VideoLibrary/VideoLibrary.API/VideoLibrary.API.csproj"
COPY . .
WORKDIR "/src/Services/VideoLibrary/VideoLibrary.API"
RUN dotnet build "./VideoLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoLibrary.API.dll"]