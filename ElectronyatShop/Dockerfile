FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /ElectronyatShop
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /ElectronyatShop/src

COPY ["ElectronyatShop.csproj", "./"]
RUN dotnet restore "ElectronyatShop.csproj"
COPY . .
RUN dotnet build "ElectronyatShop.csproj" -c $BUILD_CONFIGURATION -o /ElectronyatShop/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ElectronyatShop.csproj" -c $BUILD_CONFIGURATION -o /ElectronyatShop/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /ElectronyatShop

COPY --from=publish /ElectronyatShop/publish .
RUN mkdir -p Data

ENTRYPOINT ["sh", "-c", "dotnet ElectronyatShop.dll ef database update && dotnet ElectronyatShop.dll"]
