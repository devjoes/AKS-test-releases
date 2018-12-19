FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Canary.Management.Api/Canary.Management.Api.csproj", "Canary.Management.Api/"]
RUN dotnet restore "Canary.Management.Api/Canary.Management.Api.csproj"
COPY . .
WORKDIR "/src/Canary.Management.Api"
RUN dotnet build "Canary.Management.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Canary.Management.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Canary.Management.Api.dll"]