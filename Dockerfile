FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR build/
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
#COPY ./Smartcom.WebApp/appsettings.Secret.json .
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Smartcom.WebApp.dll"]
