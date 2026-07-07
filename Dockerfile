# ---- build ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Klippr-Backend.csproj ./
RUN dotnet restore "Klippr-Backend.csproj"

COPY . .
RUN dotnet publish "Klippr-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Correr como usuario no-root (la imagen aspnet trae el user "app", UID 1654)
USER app

ENTRYPOINT ["dotnet", "Klippr-Backend.dll"]