# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the main project file and restore dependencies
COPY ["InquiryAPI.csproj", "./"]
RUN dotnet restore "InquiryAPI.csproj"

# Copy the rest of the source code
COPY . .

# Copy and restore the additional project files
COPY ["../Application/Application.csproj", "Application/"]
COPY ["../Domain/Domain.csproj", "Domain/"]
COPY ["../Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"

# Build the project
WORKDIR "/src"
RUN dotnet build "InquiryAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "
