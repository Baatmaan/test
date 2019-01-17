FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 65279
EXPOSE 44319

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY RestApplication/RestApplication.csproj RestApplication/
RUN dotnet restore RestApplication/RestApplication.csproj
COPY . .
WORKDIR /src/RestApplication
RUN dotnet build RestApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish RestApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RestApplication.dll"]
