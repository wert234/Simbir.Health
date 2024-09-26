FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Service/Microservices/Account/AccountAPI/AccountAPI.csproj", "src/Service/Microservices/Account/AccountAPI/"]
RUN dotnet restore "./src/Service/Microservices/Account/AccountAPI/AccountAPI.csproj"
COPY . .
WORKDIR "/src/src/Service/Microservices/Account/AccountAPI"
RUN dotnet build "./AccountAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AccountAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountAPI.dll"]