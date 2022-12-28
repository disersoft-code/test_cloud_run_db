#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestCloudRunDB/TestCloudRunDB.csproj", "TestCloudRunDB/"]
COPY ["TestCloudRunDB.Data/TestCloudRunDB.Data.csproj", "TestCloudRunDB.Data/"]
COPY ["TestCloudRunDB.Model/TestCloudRunDB.Model.csproj", "TestCloudRunDB.Model/"]
RUN dotnet restore "TestCloudRunDB/TestCloudRunDB.csproj"
COPY . .
WORKDIR "/src/TestCloudRunDB"
RUN dotnet build "TestCloudRunDB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestCloudRunDB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestCloudRunDB.dll"]