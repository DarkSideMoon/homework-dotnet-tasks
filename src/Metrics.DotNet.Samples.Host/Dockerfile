#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Metrics.DotNet.Samples.Host/Metrics.DotNet.Samples.Host.csproj", "src/Metrics.DotNet.Samples.Host/"]
RUN dotnet restore "src/Metrics.DotNet.Samples.Host/Metrics.DotNet.Samples.Host.csproj"
COPY . .
WORKDIR "/src/src/Metrics.DotNet.Samples.Host"
RUN dotnet build "Metrics.DotNet.Samples.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metrics.DotNet.Samples.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Metrics.DotNet.Samples.Host.dll"]