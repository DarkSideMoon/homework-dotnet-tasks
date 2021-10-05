# build step
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# metadata
LABEL vendor="Pavlo Romashchenko (DarkSideMoon)"
LABEL authors="Pavlo Romashchenko (DarkSideMoon)"
LABEL licenses="MIT License"
LABEL description="Repository for homework tasks"
LABEL url="https://github.com/DarkSideMoon/homework-dotnet-tasks"

WORKDIR /src
COPY ["src/Homework.Dotnet.Tasks.Contracts/Homework.Dotnet.Tasks.Contracts.csproj", "src/Homework.Dotnet.Tasks.Contracts/"]
COPY ["src/Homework.Dotnet.Tasks.Documents/Homework.Dotnet.Tasks.Documents.csproj", "src/Homework.Dotnet.Tasks.Documents/"]
COPY ["src/Homework.Dotnet.Tasks.Services/Homework.Dotnet.Tasks.Services.csproj", "src/Homework.Dotnet.Tasks.Services/"]
COPY ["src/Homework.Dotnet.Tasks.Host/Homework.Dotnet.Tasks.Host.csproj", "src/Homework.Dotnet.Tasks.Host/"]

RUN dotnet restore "src/Homework.Dotnet.Tasks.Host/Homework.Dotnet.Tasks.Host.csproj"

COPY . .
WORKDIR "/src/src/Homework.Dotnet.Tasks.Host"

RUN dotnet build "Homework.Dotnet.Tasks.Host.csproj" -c Release --no-restore
RUN dotnet publish "Homework.Dotnet.Tasks.Host.csproj" -c Release -o /app/publish --no-restore --no-build

# publish step
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS publish

# Install timezones
# RUN apt-get update
# RUN apt-get install tzdata
# RUN DEBIAN_FRONTEND=noninteractive apt-get install -y tzdata

WORKDIR /app
EXPOSE 80
# EXPOSE 5000
# ENV ASPNETCORE_URLS=http://+:5000
# ENV ASPNETCORE_ENVIRONMENT=”development”
COPY --from=build /app/publish .
CMD ["dotnet", "Homework.Dotnet.Tasks.Host.dll"]