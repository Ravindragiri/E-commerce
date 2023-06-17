#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SourceFuse.E-commerce.API/SourceFuse.E-commerce.API.csproj", "SourceFuse.E-commerce.API/"]
COPY ["SourceFuse.E-commerce.Business/SourceFuse.E-commerce.Business.csproj", "SourceFuse.E-commerce.Business/"]
COPY ["SourceFuse.E-commerce.DTO/SourceFuse.E-commerce.DTO.csproj", "SourceFuse.E-commerce.DTO/"]
COPY ["SourceFuse.E-commerce.Entities/SourceFuse.E-commerce.Entities.csproj", "SourceFuse.E-commerce.Entities/"]
COPY ["SourceFuse.E-commerce.Persistence/SourceFuse.E-commerce.Persistence.csproj", "SourceFuse.E-commerce.Persistence/"]
COPY ["SourceFuse.E-commerce.Common/SourceFuse.E-commerce.Common.csproj", "SourceFuse.E-commerce.Common/"]
RUN dotnet restore "SourceFuse.E-commerce.API/SourceFuse.E-commerce.API.csproj"
COPY . .
WORKDIR "/src/SourceFuse.E-commerce.API"
RUN dotnet build "SourceFuse.E-commerce.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SourceFuse.E-commerce.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SourceFuse.E-commerce.API.dll"]