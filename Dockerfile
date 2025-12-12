# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Projeto1.csproj", "./"]
RUN dotnet restore "Projeto1.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src"
RUN dotnet build "Projeto1.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Projeto1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

# Set entrypoint
ENTRYPOINT ["dotnet", "Projeto1.dll"]

