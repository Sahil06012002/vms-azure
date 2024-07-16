#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y chromium --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/chromium"
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VendorManagementSystem.API/VendorManagementSystem.API.csproj", "VendorManagementSystem.API/"]
RUN dotnet restore "VendorManagementSystem.API/VendorManagementSystem.API.csproj"
COPY . .
WORKDIR "/src/VendorManagementSystem.API"
RUN dotnet build "VendorManagementSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VendorManagementSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:EnvironmentName=Production

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VendorManagementSystem.API.dll"]