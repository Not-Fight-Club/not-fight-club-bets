FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5007

ENV ASPNETCORE_URLS=http://+:5007

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BetsApi/BetsApi.csproj", "BetsApi/"]
RUN dotnet restore "BetsApi\BetsApi.csproj"
COPY . .
WORKDIR "/src/BetsApi"
RUN dotnet build "BetsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetsApi.dll"]
