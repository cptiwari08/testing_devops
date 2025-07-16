#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./EY.CapitalEdge.HelpCopilot /src/EY.CapitalEdge.HelpCopilot
COPY ./EY.CapitalEdge.HelpCopilot.Utils /src/EY.CapitalEdge.HelpCopilot.Utils
COPY ./EY.CapitalEdge.HelpCopilot.sln /src/EY.CapitalEdge.HelpCopilot.sln

WORKDIR /src/EY.CapitalEdge.HelpCopilot
RUN dotnet restore "./EY.CapitalEdge.HelpCopilot.csproj"
RUN dotnet build "./EY.CapitalEdge.HelpCopilot.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EY.CapitalEdge.HelpCopilot.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && apt-get clean && rm -rf /var/lib/apt/lists/*
USER app

ENTRYPOINT ["dotnet", "EY.CapitalEdge.HelpCopilot.dll"]
