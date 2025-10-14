FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore "./Wasla_Backend/Wasla_Backend.csproj"
RUN dotnet build "./Wasla_Backend/Wasla_Backend.csproj" -c Release -o /app/build
RUN dotnet publish "./Wasla_Backend/Wasla_Backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Wasla_Backend.dll"]
