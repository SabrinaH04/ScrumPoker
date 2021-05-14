#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PA.ScrumPoker.Web/PA.ScrumPoker.Web.csproj", "PA.ScrumPoker.Web/"]
RUN dotnet restore "PA.ScrumPoker.Web/PA.ScrumPoker.Web.csproj"
COPY . .
WORKDIR "/src/PA.ScrumPoker.Web"
RUN dotnet build "PA.ScrumPoker.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PA.ScrumPoker.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PA.ScrumPoker.Web.dll"]