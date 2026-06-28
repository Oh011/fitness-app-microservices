# Stage 1: Base - Setup runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Stage 2: Build - Restore and compile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project files for restore
COPY ["src/WorkoutService/WorkoutService.Api/WorkoutService.Api.csproj", "src/WorkoutService/WorkoutService.Api/"]
COPY ["src/WorkoutService/WorkoutService.Application/WorkoutService.Application.csproj", "src/WorkoutService/WorkoutService.Application/"]
COPY ["src/WorkoutService/WorkoutService.Domain/WorkoutService.Domain.csproj", "src/WorkoutService/WorkoutService.Domain/"]
COPY ["src/WorkoutService/WorkoutService.Infrastructure/WorkoutService.Infrastructure.csproj", "src/WorkoutService/WorkoutService.Infrastructure/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]

# Restore
RUN dotnet restore "src/WorkoutService/WorkoutService.Api/WorkoutService.Api.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/WorkoutService/WorkoutService.Api"
RUN dotnet build "WorkoutService.Api.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WorkoutService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final - Run the service
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkoutService.Api.dll"]
