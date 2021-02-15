FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Checkers.Api.csproj", "./"]
RUN dotnet restore "Checkers.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Checkers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkers.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkers.Api.dll"]
