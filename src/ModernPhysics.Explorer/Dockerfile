FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/ModernPhysics.Explorer/ModernPhysics.Explorer.csproj", "src/ModernPhysics.Explorer/"]
RUN dotnet restore "src/ModernPhysics.Explorer/ModernPhysics.Explorer.csproj"
COPY . .
WORKDIR "/src/src/ModernPhysics.Explorer"
RUN dotnet build "ModernPhysics.Explorer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ModernPhysics.Explorer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ModernPhysics.Explorer.dll"]