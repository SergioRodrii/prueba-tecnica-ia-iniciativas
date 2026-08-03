# Stage 1: Build y compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BackendDotnet.csproj", "./"]
RUN dotnet restore "BackendDotnet.csproj"

COPY . .
RUN dotnet publish "BackendDotnet.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime liviano de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001

ENTRYPOINT ["dotnet", "BackendDotnet.dll"]
