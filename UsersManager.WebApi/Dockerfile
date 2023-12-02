FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UsersManager.WebApi/UsersManager.WebApi.csproj", "UsersManager.WebApi/"]
RUN dotnet restore "UsersManager.WebApi/UsersManager.WebApi.csproj"
COPY . .
WORKDIR "/src/UsersManager.WebApi"
RUN dotnet build "UsersManager.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersManager.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersManager.WebApi.dll"]
