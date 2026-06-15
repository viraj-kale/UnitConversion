FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["UnitConversion/UnitConversion.csproj", "UnitConversion/"]
COPY ["UnitConversion.Tests/UnitConversion.Tests.csproj", "UnitConversion.Tests/"]
COPY ["UnitConversion.sln", "./"]
RUN dotnet restore "UnitConversion/UnitConversion.csproj"

COPY . .
WORKDIR /src/UnitConversion
RUN dotnet publish "UnitConversion.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && adduser --disabled-password --gecos "" appuser \
    && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "UnitConversion.dll"]
