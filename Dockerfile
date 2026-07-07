FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia los archivos del proyecto apuntando directamente a la estructura
COPY CoreFlow_Backend.csproj ./
RUN dotnet restore

# Copia el resto del código
COPY . ./
RUN dotnet publish "CoreFlow_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "CoreFlow_Backend.dll"]