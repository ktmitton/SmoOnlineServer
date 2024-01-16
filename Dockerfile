FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 5217
EXPOSE 5486

ENV ASPNETCORE_URLS=http://+:5217
ENV Lobby__Lobbies__0__Name="Hide & Seek"
ENV Lobby__Lobbies__0__Type="HideAndSeek"
ENV Lobby__Lobbies__0__Url="tcp://*:5486"

USER app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG configuration=Release
WORKDIR /src
COPY ["SuperMarioOdysseyOnline.Server.Application/SuperMarioOdysseyOnline.Server.Application.csproj", "SuperMarioOdysseyOnline.Server.Application/"]
COPY ["SuperMarioOdysseyOnline.Server.Core/SuperMarioOdysseyOnline.Server.Core.csproj", "SuperMarioOdysseyOnline.Server.Core/"]
COPY ["SuperMarioOdysseyOnline.Server.Lobbies/SuperMarioOdysseyOnline.Server.Lobbies.csproj", "SuperMarioOdysseyOnline.Server.Lobbies/"]
RUN dotnet restore "SuperMarioOdysseyOnline.Server.Application/SuperMarioOdysseyOnline.Server.Application.csproj"
COPY . .
WORKDIR "/src/SuperMarioOdysseyOnline.Server.Application"
RUN dotnet build "SuperMarioOdysseyOnline.Server.Application.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SuperMarioOdysseyOnline.Server.Application.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SuperMarioOdysseyOnline.Server.Application.dll"]
