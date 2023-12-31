# Use the ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the main csproj file and restore as distinct layers
COPY ["InquiryStatusUpdater.csproj", "./"]
RUN dotnet restore "InquiryStatusUpdater.csproj"

# Copy the rest of the source code
COPY . .

# Copy the other project files and restore them
COPY ["../Application/Application.csproj", "Application/"]
COPY ["../Domain/Domain.csproj", "Domain/"]
COPY ["../Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"

# Build the project
WORKDIR "/src"
RUN dotnet build "InquiryStatusUpdater.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "InquiryStatusUpdater.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InquiryStatusUpdater.dll"]
