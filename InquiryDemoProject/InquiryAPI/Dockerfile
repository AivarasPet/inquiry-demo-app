# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the .csproj files first and restore to cache the layers
COPY ["InquiryAPI/InquiryAPI.csproj", "InquiryAPI/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies for each project
RUN dotnet restore "InquiryAPI/InquiryAPI.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"

# Copy the source code for each project
COPY Domain/ Domain/
COPY Application/ Application/
COPY Infrastructure/ Infrastructure/
COPY InquiryAPI/ InquiryAPI/

# Set the working directory to the main project
WORKDIR "/src/InquiryAPI"

# Build the project
RUN dotnet build "InquiryAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "InquiryAPI.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InquiryAPI.dll"]
