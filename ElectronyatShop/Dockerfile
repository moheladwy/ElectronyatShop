FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /ElectronyatShop
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /ElectronyatShop/src
COPY . .
RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /ElectronyatShop/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /ElectronyatShop/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /ElectronyatShop
COPY --from=publish /ElectronyatShop/publish .
ENTRYPOINT ["dotnet", "ElectronyatShop.dll"]
