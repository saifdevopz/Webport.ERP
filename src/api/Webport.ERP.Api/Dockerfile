# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/api/Webport.ERP.Api/Webport.ERP.Api.csproj", "src/api/Webport.ERP.Api/"]
COPY ["src/modules/identity/Webport.ERP.Identity.Infrastructure/Webport.ERP.Identity.Infrastructure.csproj", "src/modules/identity/Webport.ERP.Identity.Infrastructure/"]
COPY ["src/common/Webport.ERP.Common.Infrastructure/Webport.ERP.Common.Infrastructure.csproj", "src/common/Webport.ERP.Common.Infrastructure/"]
COPY ["src/common/Webport.ERP.Common.Application/Webport.ERP.Common.Application.csproj", "src/common/Webport.ERP.Common.Application/"]
COPY ["src/common/Webport.ERP.Common.Domain/Webport.ERP.Common.Domain.csproj", "src/common/Webport.ERP.Common.Domain/"]
COPY ["src/modules/identity/Webport.ERP.Identity.Application/Webport.ERP.Identity.Application.csproj", "src/modules/identity/Webport.ERP.Identity.Application/"]
COPY ["src/modules/identity/Webport.ERP.Identity.Domain/Webport.ERP.Identity.Domain.csproj", "src/modules/identity/Webport.ERP.Identity.Domain/"]
COPY ["src/modules/identity/Webport.ERP.Identity.Presentation/Webport.ERP.Identity.Presentation.csproj", "src/modules/identity/Webport.ERP.Identity.Presentation/"]
COPY ["src/common/Webport.ERP.Common.Presentation/Webport.ERP.Common.Presentation.csproj", "src/common/Webport.ERP.Common.Presentation/"]
COPY ["src/modules/inventory/Webport.ERP.Inventory.Infrastructure/Webport.ERP.Inventory.Infrastructure.csproj", "src/modules/inventory/Webport.ERP.Inventory.Infrastructure/"]
COPY ["src/modules/inventory/Webport.ERP.Inventory.Application/Webport.ERP.Inventory.Application.csproj", "src/modules/inventory/Webport.ERP.Inventory.Application/"]
COPY ["src/modules/inventory/Webport.ERP.Inventory.Domain/Webport.ERP.Inventory.Domain.csproj", "src/modules/inventory/Webport.ERP.Inventory.Domain/"]
COPY ["src/modules/inventory/Webport.ERP.Inventory.Presentation/Webport.ERP.Inventory.Presentation.csproj", "src/modules/inventory/Webport.ERP.Inventory.Presentation/"]
RUN dotnet restore "./src/api/Webport.ERP.Api/Webport.ERP.Api.csproj"
COPY . .
WORKDIR "/src/src/api/Webport.ERP.Api"
RUN dotnet build "./Webport.ERP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Webport.ERP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webport.ERP.Api.dll"]