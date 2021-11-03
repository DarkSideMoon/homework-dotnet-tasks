#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Homework.Dotnet.Tasks.Weather.Host/Homework.Dotnet.Tasks.Weather.Host.csproj", "src/Homework.Dotnet.Tasks.Weather.Host/"]
RUN dotnet restore "src/Homework.Dotnet.Tasks.Weather.Host/Homework.Dotnet.Tasks.Weather.Host.csproj"
COPY . .
WORKDIR "/src/src/Homework.Dotnet.Tasks.Weather.Host"
RUN dotnet build "Homework.Dotnet.Tasks.Weather.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Homework.Dotnet.Tasks.Weather.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Homework.Dotnet.Tasks.Weather.Host.dll"]