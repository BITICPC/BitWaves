FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
ARG BUILD_TYPE=Release
WORKDIR /app

# Copy project metadata and restore as distinct layers.
COPY BitWaves.WebAPI/BitWaves.WebAPI.csproj ./BitWaves.WebAPI/
COPY BitWaves.Data/BitWaves.Data/BitWaves.Data.csproj ./BitWaves.Data/BitWaves.Data/
WORKDIR /app/BitWaves.WebAPI
RUN dotnet restore

# Copy everything else and build app.
WORKDIR /app
COPY BitWaves.WebAPI/. ./BitWaves.WebAPI/
COPY BitWaves.Data/BitWaves.Data/ ./BitWaves.Data/BitWaves.Data/
WORKDIR /app/BitWaves.WebAPI
RUN dotnet publish -c $BUILD_TYPE -o publish


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
ARG HOST_ENV=Production
WORKDIR /app
COPY --from=build /app/BitWaves.WebAPI/publish ./
ENV ASPNETCORE_ENVIRONMENT=$HOST_ENV
ENTRYPOINT ["dotnet", "BitWaves.WebAPI.dll"]
EXPOSE 80 443
