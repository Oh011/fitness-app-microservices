# Stage 1: Base - Setup runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Stage 2: Build - Restore and compile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

# ... inside the build stage ...
WORKDIR /src

# Copy all project files first (maintaining the directory tree)
COPY ["src/AuthenticationService/AuthenticationService.Api/AuthenticationService.Api.csproj", "src/AuthenticationService/AuthenticationService.Api/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]

# Restore
RUN dotnet restore "src/AuthenticationService/AuthenticationService.Api/AuthenticationService.Api.csproj"

# NOW copy everything else
COPY . .

# Build
WORKDIR "/src/src/AuthenticationService/AuthenticationService.Api"
RUN dotnet build "AuthenticationService.Api.csproj" -c Release -o /app/build



# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthenticationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final - Run the service
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.Api.dll"]