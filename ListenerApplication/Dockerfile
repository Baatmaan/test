FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 65342
EXPOSE 44340

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ListenerApplication/ListenerApplication.csproj ListenerApplication/
RUN dotnet restore ListenerApplication/ListenerApplication.csproj
COPY . .
WORKDIR /src/ListenerApplication
RUN dotnet build ListenerApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ListenerApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ListenerApplication.dll"]
