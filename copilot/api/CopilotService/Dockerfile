#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Enable contrib repository and install fontconfig and Microsoft core fonts
RUN echo "deb http://deb.debian.org/debian/ bullseye main contrib non-free" > /etc/apt/sources.list && \
    echo "deb-src http://deb.debian.org/debian/ bullseye main contrib non-free" >> /etc/apt/sources.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends \
        fontconfig \
        ttf-mscorefonts-installer \		
    && rm -rf /var/lib/apt/lists/*
USER app
WORKDIR /app
EXPOSE 8080
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EY.CE.Copilot.API/EY.CE.Copilot.API.csproj", "EY.CE.Copilot.API/"]
COPY ["EY.CE.Copilot.Data/EY.CE.Copilot.Data.csproj", "EY.CE.Copilot.Data/"]
COPY ["Offline.Nuget.Packages/EY.SAT.CE.CoreServices.2.0.0.nupkg", "Offline.Nuget.Packages/"]
COPY ["Offline.Nuget.Packages/EY.SaT.CapitalEdge.Extensions.Logging.1.0.0.nupkg", "Offline.Nuget.Packages/"]
COPY ["Offline.Nuget.Packages/EY.SAT.CE.SharePoint.1.0.0.nupkg", "Offline.Nuget.Packages/"]
COPY ./nuget.config ./
RUN dotnet restore "./EY.CE.Copilot.API/EY.CE.Copilot.API.csproj"
COPY . .
WORKDIR "/src/EY.CE.Copilot.API"
RUN dotnet build "./EY.CE.Copilot.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EY.CE.Copilot.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS dev
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EY.CE.Copilot.API.dll"]

FROM base AS prod
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EY.CE.Copilot.API.dll"]
