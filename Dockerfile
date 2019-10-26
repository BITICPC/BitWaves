FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /app

# Copy project metadata and restore as distinct layers.
COPY *.sln .
COPY BitWaves.WebAPI/BitWaves.WebAPI.csproj ./BitWaves.WebAPI/
COPY BitWaves.Data/BitWaves.Data.csproj ./BitWaves.Data/
COPY BitWaves.UnitTest/BitWaves.UnitTest.csproj ./BitWaves.UnitTest/
RUN dotnet restore

# Copy everything else and build app.
COPY BitWaves.WebAPI/. ./BitWaves.WebAPI/
COPY BitWaves.Data/. ./BitWaves.Data/
COPY BitWaves.UnitTest/. ./BitWaves.UnitTest/
WORKDIR /app/BitWaves.WebAPI
RUN dotnet publish -c Release -o publish


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
WORKDIR /app
COPY --from=build /app/BitWaves.WebAPI/publish ./
ENTRYPOINT ["dotnet", "BitWaves.WebAPI.dll"]
EXPOSE 80 443
