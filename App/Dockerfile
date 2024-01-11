#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# #EXPOSE 80
# EXPOSE 5108

# # Copy csproj and restore as distinct layers
# COPY *.csproj ./
# #RUN dotnet restore "DotNet.Docker.csproj"
# RUN dotnet restore

# # Copy everything else and build the application
# COPY . .
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# WORKDIR /app
# COPY --from=build /app/out .
# ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]

#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["DotNet.Docker.csproj", "./"]
# RUN dotnet restore "DotNet.Docker.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "DotNet.Docker.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "DotNet.Docker.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]