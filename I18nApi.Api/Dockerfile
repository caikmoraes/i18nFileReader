FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["I18nApi.Api/I18nApi.Api.csproj", "I18nApi.Api/"]
COPY ["I18nApi.Application/I18nApi.Application.csproj", "I18nApi.Application/"]
COPY ["I18nApi.Domain/I18nApi.Domain.csproj", "I18nApi.Domain/"]
COPY ["I18nApi.Infrastructure/I18nApi.Infrastructure.csproj", "I18nApi.Infrastructure/"]
COPY ["I18nApi.Contracts/I18nApi.Contracts.csproj", "I18nApi.Contracts/"]
RUN dotnet restore "I18nApi.Api/I18nApi.Api.csproj"
COPY . .
WORKDIR "/src/I18nApi.Api"
RUN dotnet build "I18nApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "I18nApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "I18nApi.Api.dll"]
