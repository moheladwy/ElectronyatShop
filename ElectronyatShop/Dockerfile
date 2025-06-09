FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /ElectronyatShop
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ElectronyatShop/ElectronyatShop.csproj", "ElectronyatShop/"]
RUN dotnet restore "ElectronyatShop/ElectronyatShop.csproj"
COPY . .
WORKDIR "/src/ElectronyatShop"
RUN dotnet build "./ElectronyatShop.csproj" -c $BUILD_CONFIGURATION -o /ElectronyatShop/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ElectronyatShop.csproj" -c $BUILD_CONFIGURATION -o /ElectronyatShop/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /ElectronyatShop
COPY --from=publish /ElectronyatShop/publish .
ENTRYPOINT ["dotnet", "ElectronyatShop.dll"]
