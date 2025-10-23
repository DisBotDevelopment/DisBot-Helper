FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DisBot-Helper-Bot/DisBot-Helper-Bot.csproj", "DisBot-Helper-Bot/"]
RUN dotnet restore "DisBot-Helper-Bot/DisBot-Helper-Bot.csproj"
COPY . .
WORKDIR "/src/DisBot-Helper-Bot"
RUN dotnet publish "./DisBot-Helper-Bot.csproj" -c Release -r linux-x64 \
                --self-contained true \
                -p:PublishReadyToRun=true \
                -p:PublishSingleFile=true \
                -o /app/publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DisBot-Helper-Bot.csproj" -c Release -r linux-x64 \
                --self-contained true \
                -p:PublishReadyToRun=true \
                -p:PublishSingleFile=true \
                -o /app/publish
FROM base AS final
WORKDIR /app
COPY /app/publish .
RUN chmod +x ./DisBot-Helper-Bot
CMD ["./DisBot-Helper-Bot"]
