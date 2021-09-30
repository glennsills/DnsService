#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 53531

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DnsService/DnsService.csproj", "DnsService/"]
RUN dotnet restore "DnsService/DnsService.csproj"
COPY . .
WORKDIR "/src/DnsService"
RUN dotnet build "DnsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DnsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DnsService.dll"]