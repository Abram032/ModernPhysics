FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 3306

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["src/ModernPhysics.Web/ModernPhysics.Web.csproj", "src/ModernPhysics.Web/"]
COPY ["lib/ModernPhysics.Models/ModernPhysics.Models.csproj", "lib/ModernPhysics.Models/"]
RUN dotnet restore "src/ModernPhysics.Web/ModernPhysics.Web.csproj"
COPY . .
WORKDIR "/src/src/ModernPhysics.Web"
RUN dotnet build "ModernPhysics.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ModernPhysics.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ModernPhysics.Web.dll"]