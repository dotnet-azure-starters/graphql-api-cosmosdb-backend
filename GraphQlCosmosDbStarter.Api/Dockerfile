#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GraphQlCosmosDbStarter.Api/GraphQlCosmosDbStarter.Api.csproj", "GraphQlCosmosDbStarter.Api/"]
COPY ["GraphQlCosmosDbStarter.Data/GraphQlCosmosDbStarter.Data.csproj", "GraphQlCosmosDbStarter.Data/"]
RUN dotnet restore "GraphQlCosmosDbStarter.Api/GraphQlCosmosDbStarter.Api.csproj"
COPY . .
WORKDIR "/src/GraphQlCosmosDbStarter.Api"
RUN dotnet build "GraphQlCosmosDbStarter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQlCosmosDbStarter.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQlCosmosDbStarter.Api.dll"]