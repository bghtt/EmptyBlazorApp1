FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EmptyBlazorApp1/EmptyBlazorApp1.csproj", "EmptyBlazorApp1/"]
RUN dotnet restore "EmptyBlazorApp1/EmptyBlazorApp1.csproj"
COPY . .
WORKDIR "/src/EmptyBlazorApp1"
RUN dotnet build "EmptyBlazorApp1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmptyBlazorApp1.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Устанавливаем инструменты для устранения проблем (можно удалить в production)
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "EmptyBlazorApp1.dll"]