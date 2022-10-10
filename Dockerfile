# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./src/Lopes.Jobs.Api/Lopes.Jobs.Api.csproj" --disable-parallel
RUN dotnet publish "./src/Lopes.Jobs.Api/Lopes.Jobs.Api.csproj" -c release -o /app --no-restore

#Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "Lopes.Jobs.Api.dll"]