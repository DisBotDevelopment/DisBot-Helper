FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DisBot-Helper-Bot/DisBot-Helper-Bot.csproj", "DisBot-Helper-Bot/"]
RUN dotnet restore "DisBot-Helper-Bot/DisBot-Helper-Bot.csproj"
COPY . .
WORKDIR "/src/DisBot-Helper-Bot"
RUN dotnet build "./DisBot-Helper-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DisBot-Helper-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DisBot-Helper-Bot.dll"]
