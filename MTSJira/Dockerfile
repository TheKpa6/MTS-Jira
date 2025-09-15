FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS base
RUN apt update && apt upgrade -y && apt install -y qttools5-dev-tools
EXPOSE 44392

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
ARG BUILD_CONFIGURATION=Release
COPY ["/src", "/src"]
WORKDIR "/src/MTSJira.Api"
RUN dotnet publish "MTSJira.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Prod
ENV ASPNETCORE_HTTPS_PORT=44392
ENV ASPNETCORE_URLS=https://+:44392;http://+:8080
ENV Kestrel__Certificates__Default__Path=/root/.dotnet/https/cert-aspnetcore.pfx 
ENV Kestrel__Certificates__Default__Password=ufo 
COPY ["/src/Dev.Cert/cert-aspnetcore.pfx", "/root/.dotnet/https/"]
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MTSJira.Api.dll"]    